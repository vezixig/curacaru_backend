namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Services;

/// <summary>Request to add a new appointment.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="appointment">Appointment data.</param>
public class AddAppointmentRequest(User user, AddAppointmentDto appointment) : IRequest<GetAppointmentDto>
{
    /// <summary>Gets the appointment data.</summary>
    public AddAppointmentDto Appointment { get; } = appointment;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class AddAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IBudgetRepository budgetRepository,
    IBudgetService budgetService,
    ICustomerRepository customerRepository,
    IDatabaseService databaseService,
    IDateTimeService dateTimeService,
    IEmployeeRepository employeeRepository,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<AddAppointmentRequest, GetAppointmentDto>
{
    public async Task<GetAppointmentDto> Handle(AddAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = mapper.Map<Appointment>(request.Appointment);
        appointment.CompanyId = request.User.CompanyId;

        // Auth
        if (request.User.EmployeeId != appointment.EmployeeId && !request.User.IsManager)
            throw new ForbiddenException("Nur Manager dürfen für andere Mitarbeiter Termine anlegen.");
        if (appointment.EmployeeReplacementId.HasValue && !request.User.IsManager) throw new ForbiddenException("Nur Manager dürfen Vertretungen einsetzen.");

        // Date
        if (request.Appointment.Date < new DateOnly(dateTimeService.Today.Year, dateTimeService.Now.Month, 1))
            throw new BadRequestException("Termine können nicht vor dem aktuellen Monat liegen.");
        appointment.IsPlanned = appointment.Date > dateTimeService.EndOfMonth;

        // Clearance type
        if (request.Appointment.Date < new DateOnly(dateTimeService.Today.Year, dateTimeService.Today.Month, 1).AddMonths(1)
            && request.Appointment.ClearanceType is null)
            throw new BadRequestException("Für diesen Termin muss eine Abrechnungsoption gewählt werden.");

        // customer
        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.Appointment.CustomerId)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");

        if (customer.Status != CustomerStatus.Customer) throw new BadRequestException("Kunde ist nicht aktiv.");

        if (request.User.EmployeeId != customer.AssociatedEmployeeId && !request.User.IsManager)
            throw new ForbiddenException("Nur Manager dürfen Termine für nicht selbst betreute Kunden anlegen.");
        appointment.Customer = new() { Id = customer.Id };

        // employee
        var employee = await employeeRepository.GetEmployeeByIdAsync(request.User.CompanyId, request.Appointment.EmployeeId)
                       ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");
        appointment.Employee = new() { Id = employee.Id };

        // employee replacement
        if (appointment.EmployeeReplacementId.HasValue)
        {
            var employeeReplacement = await employeeRepository.GetEmployeeByIdAsync(request.User.CompanyId, request.Appointment.EmployeeReplacementId!.Value)
                                      ?? throw new NotFoundException("Vertretung nicht gefunden.");
            appointment.EmployeeReplacement = new() { Id = employeeReplacement.Id };
        }

        // overlapping
        var isOverlapping = await mediator.Send(
            new IsBlockingAppointmentRequest(
                request.User.CompanyId,
                request.Appointment.EmployeeReplacementId ?? request.Appointment.EmployeeId,
                request.Appointment.Date,
                request.Appointment.TimeStart,
                request.Appointment.TimeEnd,
                null),
            cancellationToken);
        if (isOverlapping) throw new BadRequestException("Der Termin überschneidet sich mit einem anderen Termin.");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);
        try
        {
            if (!appointment.IsPlanned) await ProcessBudget(customer, appointment);

            appointment = await appointmentRepository.AddAppointmentAsync(appointment);
            await transaction.CommitAsync(cancellationToken);
            return mapper.Map<GetAppointmentDto>(appointment);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    private async Task ProcessBudget(Customer customer, Appointment appointment)
    {
        var price = await budgetService.CalculateAppointmentPriceAsync(PriceCalculationData.CreateFrom(appointment));
        var budget = await budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, appointment.CustomerId)
                     ?? new Budget { Customer = customer };
        switch (appointment.ClearanceType)
        {
            case ClearanceType.CareBenefit:
                if (price > budget.CareBenefitAmount) throw new BadRequestException("Budget ist überschritten.");
                budget.CareBenefitAmount -= price;
                appointment.Costs = price;
                break;
            case ClearanceType.PreventiveCare:
                if (price > budget.PreventiveCareAmount) throw new BadRequestException("Budget ist überschritten.");
                budget.PreventiveCareAmount -= price;
                appointment.Costs = price;
                break;
            case ClearanceType.ReliefAmount:
                if (price > budget.ReliefAmount + budget.ReliefAmountLastYear) throw new BadRequestException("Budget ist überschritten.");
                budget.ReliefAmountLastYear -= price;
                appointment.CostsLastYearBudget += price;
                if (budget.ReliefAmountLastYear < 0)
                {
                    budget.ReliefAmount += budget.ReliefAmountLastYear;
                    appointment.CostsLastYearBudget += budget.ReliefAmountLastYear;
                    appointment.Costs += budget.ReliefAmountLastYear * -1;
                    budget.ReliefAmountLastYear = 0;
                }

                break;
            case ClearanceType.SelfPayment:
                if (price > budget.SelfPayAmount) throw new BadRequestException("Budget ist überschritten.");
                budget.SelfPayAmount -= price;
                appointment.Costs += price;
                break;
        }

        await budgetRepository.UpdateBudgetAsync(budget);
    }
}