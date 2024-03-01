namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;
using Services;

public class UpdateAppointmentRequest(Guid companyId, string authId, UpdateAppointmentDto appointment) : IRequest<GetAppointmentDto>
{
    public UpdateAppointmentDto Appointment { get; } = appointment;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class UpdateAppointmentRequestHandler(
    IAppointmentRepository repository,
    IBudgetRepository budgetRepository,
    IBudgetService budgetService,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<UpdateAppointmentRequest, GetAppointmentDto>
{
    public async Task<GetAppointmentDto> Handle(UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await repository.GetAppointmentAsync(request.CompanyId, request.Appointment.Id)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        if (user!.Id != appointment.EmployeeId && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen fremde Termine bearbeiten.");

        // customer
        if (appointment.CustomerId != request.Appointment.CustomerId)
        {
            if (!user.IsManager) throw new ForbiddenException("Nur Manager dürfen den Kunden ändern.");

            var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.Appointment.CustomerId)
                           ?? throw new BadRequestException("Kunde nicht gefunden.");
            appointment.CustomerId = customer.Id;
        }

        // employee

        if (appointment.EmployeeId != request.Appointment.EmployeeId)
        {
            if (!user!.IsManager) throw new ForbiddenException("Nur Manager dürfen den Mitarbeiter ändern.");

            var employee = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.Appointment.EmployeeId)
                           ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");
            appointment.EmployeeId = employee.Id;
        }

        // employee replacement
        if (appointment.EmployeeReplacementId != request.Appointment.EmployeeReplacementId)
        {
            if (!user!.IsManager) throw new ForbiddenException("Nur Manager dürfen die Vertretung ändern.");

            if (request.Appointment.EmployeeReplacementId == null) { appointment.EmployeeReplacementId = null; }
            else
            {
                var employeeReplacement = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.Appointment.EmployeeReplacementId.Value)
                                          ?? throw new NotFoundException("Vertretung nicht gefunden.");
                appointment.EmployeeReplacementId = employeeReplacement.Id;
            }
        }

        // calculate new price
        var price = await budgetService.CalculateAppointmentPriceAsync(appointment);
        if (request.Appointment.ClearanceType != appointment.ClearanceType)
        {
            // clearance type changed - refund to old clearance type
            await budgetService.RefundBudget(appointment);
        }
        else if (price < appointment.Costs + appointment.CostsLastYearBudget)
        {
            // appointment got cheaper - refund difference
            var refundAppointment = mapper.Map<Appointment>(appointment);
            refundAppointment.Costs -= price;
            if (refundAppointment.Costs < 0)
            {
                refundAppointment.CostsLastYearBudget -= refundAppointment.Costs;
                refundAppointment.Costs = 0;
            }

            await budgetService.RefundBudget(appointment);
            appointment.Costs -= refundAppointment.Costs;
            appointment.CostsLastYearBudget -= refundAppointment.CostsLastYearBudget;
        }
        else if (price > appointment.Costs + appointment.CostsLastYearBudget)
        {
            // appointment got more expensive - charge difference
            price -= appointment.Costs + appointment.CostsLastYearBudget;
            await ProcessBudget(price, appointment);
        }

        // refund budget if necessary

        appointment.ClearanceType = request.Appointment.ClearanceType;
        appointment.Date = request.Appointment.Date;
        appointment.DistanceToCustomer = request.Appointment.DistanceToCustomer;
        appointment.TimeStart = request.Appointment.TimeStart;
        appointment.TimeEnd = request.Appointment.TimeEnd;
        appointment.IsSignedByCustomer = request.Appointment.IsSignedByCustomer;
        appointment.IsSignedByEmployee = request.Appointment.IsSignedByEmployee;
        appointment.Notes = request.Appointment.Notes;

        appointment.Customer = new() { Id = appointment.CustomerId };
        appointment.Employee = new() { Id = appointment.EmployeeId };
        appointment.EmployeeReplacement = appointment.EmployeeReplacementId.HasValue
            ? new Employee { Id = appointment.EmployeeReplacementId.Value }
            : null;

        var updatedAppointment = await repository.UpdateAppointmentAsync(appointment);
        return mapper.Map<GetAppointmentDto>(updatedAppointment);
    }

    private async Task ProcessBudget(decimal priceRaise, Appointment appointment)
    {
        var budget = await budgetRepository.GetCurrentBudgetAsync(appointment.CompanyId, appointment.CustomerId)
                     ?? new Budget { Customer = appointment.Customer };

        switch (appointment.ClearanceType)
        {
            case ClearanceType.CareBenefit:
                if (priceRaise > budget.CareBenefitAmount) throw new BadRequestException("Budget ist überschritten.");
                budget.CareBenefitAmount -= priceRaise;
                appointment.Costs += priceRaise;
                break;
            case ClearanceType.PreventiveCare:
                if (priceRaise > budget.PreventiveCareAmount) throw new BadRequestException("Budget ist überschritten.");
                budget.PreventiveCareAmount -= priceRaise;
                appointment.Costs += priceRaise;
                break;
            case ClearanceType.ReliefAmount:
                if (priceRaise > budget.ReliefAmount + budget.ReliefAmountLastYear) throw new BadRequestException("Budget ist überschritten.");
                budget.ReliefAmountLastYear -= priceRaise;
                appointment.CostsLastYearBudget += priceRaise;
                if (budget.ReliefAmountLastYear < 0)
                {
                    budget.ReliefAmount += budget.ReliefAmountLastYear;
                    appointment.CostsLastYearBudget += budget.ReliefAmountLastYear;
                    appointment.Costs += budget.ReliefAmountLastYear * -1;
                    budget.ReliefAmountLastYear = 0;
                }

                break;
            case ClearanceType.SelfPayment:
                if (priceRaise > budget.SelfPayAmount) throw new BadRequestException("Budget ist überschritten.");
                budget.SelfPayAmount -= priceRaise;
                appointment.Costs = priceRaise;
                break;
        }

        await budgetRepository.UpdateBudgetAsync(budget);
    }
}