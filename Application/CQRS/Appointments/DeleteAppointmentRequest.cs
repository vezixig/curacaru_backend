namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Services;
using Appointment = Core.Entities.Appointment;

/// <summary>Request to delete an appointment.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="authId">The auth id of the user.</param>
/// <param name="appointmentId">The appointment id.</param>
public class DeleteAppointmentRequest(Guid companyId, string authId, Guid appointmentId) : IRequest
{
    public Guid AppointmentId { get; } = appointmentId;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class DeleteAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    IDatabaseService databaseService,
    IDateTimeService dateTimeService,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<DeleteAppointmentRequest>
{
    public async Task Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != appointment.EmployeeId && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen fremde Termine löschen.");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            await ProcessBudgetAsync(appointment);

            await appointmentRepository.DeleteAppointmentAsync(appointment);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    internal async Task ProcessBudgetAsync(Appointment appointment)
    {
        var budget = await budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, appointment.CustomerId)
                     ?? throw new BadRequestException("Budget nicht gefunden.");

        switch (appointment.ClearanceType)
        {
            case ClearanceType.SelfPayment:
                if (appointment.Date.Year != dateTimeService.Now.Year || appointment.Date.Month != dateTimeService.Now.Month) return;
                budget.SelfPayAmount += appointment.Costs;
                break;
            case ClearanceType.CareBenefit:
                if (appointment.Date.Year != dateTimeService.Now.Year || appointment.Date.Month != dateTimeService.Now.Month) return;
                budget.CareBenefitAmount += appointment.Costs;
                break;
            case ClearanceType.PreventiveCare:
                if (appointment.Date.Year != dateTimeService.Now.Year) return;
                budget.PreventiveCareAmount += appointment.Costs;
                break;
            case ClearanceType.ReliefAmount:
                // Check if budget from previous year is expired
                if (appointment.Date.Year < dateTimeService.Now.Year - 1 || (appointment.Date.Year < dateTimeService.Now.Year && dateTimeService.Now.Month > 6))
                    return;

                // Budget from two years ago is expired - switch to process
                if (appointment.Date.Year < dateTimeService.Now.Year)
                {
                    appointment.CostsLastYearBudget = appointment.Costs;
                    appointment.Costs = 0;
                }

                var customerAppointments = await appointmentRepository.GetAppointmentsAsync(
                    appointment.CompanyId,
                    new DateOnly(dateTimeService.Now.Year, 1, 1),
                    new DateOnly(dateTimeService.Now.Year, 12, 31),
                    null,
                    appointment.CustomerId);

                // remove deleted appointment and all appointments that can not be booked on last year budget
                customerAppointments.RemoveAll(o => o.Id == appointment.Id || o.Costs == 0);

                var appointmentIndex = 0;

                // refund budget from previous year
                while (appointment.CostsLastYearBudget > 0 && appointmentIndex < customerAppointments.Count)
                {
                    var refundAppointment = customerAppointments[appointmentIndex];
                    var refunded = Math.Min(refundAppointment.Costs, appointment.CostsLastYearBudget);

                    refundAppointment.CostsLastYearBudget += refunded;
                    refundAppointment.Costs -= refunded;
                    appointment.CostsLastYearBudget -= refunded;

                    await appointmentRepository.UpdateAppointmentAsync(refundAppointment);
                    appointmentIndex++;
                }

                budget.ReliefAmount += appointment.Costs;
                budget.ReliefAmountLastYear += appointment.CostsLastYearBudget;

                break;
        }

        await budgetRepository.UpdateBudgetAsync(budget);
    }
}