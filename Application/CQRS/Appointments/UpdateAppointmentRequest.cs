namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO;
using Core.DTO.Appointment;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class UpdateAppointmentRequest(Guid companyId, string authId, UpdateAppointmentDto appointment) : IRequest<GetAppointmentDto>
{
    public UpdateAppointmentDto Appointment { get; } = appointment;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class UpdateAppointmentRequestHandler(
    IAppointmentRepository repository,
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

        appointment.Date = request.Appointment.Date;
        appointment.TimeStart = request.Appointment.TimeStart;
        appointment.TimeEnd = request.Appointment.TimeEnd;
        appointment.IsSignedByCustomer = request.Appointment.IsSignedByCustomer;
        appointment.IsSignedByEmployee = request.Appointment.IsSignedByEmployee;
        appointment.Notes = request.Appointment.Notes;

        appointment.Customer = new Customer { Id = appointment.CustomerId };
        appointment.Employee = new Employee { Id = appointment.EmployeeId };
        appointment.EmployeeReplacement = appointment.EmployeeReplacementId.HasValue
            ? new Employee { Id = appointment.EmployeeReplacementId.Value }
            : null;

        var updatedAppointment = await repository.UpdateAppointmentAsync(appointment);
        return mapper.Map<GetAppointmentDto>(updatedAppointment);
    }
}