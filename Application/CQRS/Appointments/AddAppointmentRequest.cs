namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class AddAppointmentRequest(Guid companyId, string authId, AddAppointmentDto appointment) : IRequest<GetAppointmentDto>
{
    public AddAppointmentDto Appointment { get; } = appointment;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class AddAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<AddAppointmentRequest, GetAppointmentDto>
{
    public async Task<GetAppointmentDto> Handle(AddAppointmentRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var appointment = mapper.Map<Appointment>(request.Appointment);
        appointment.CompanyId = request.CompanyId;

        // Auth
        if (user!.Id != appointment.EmployeeId && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen für andere Mitarbeiter Termine anlegen.");
        if (appointment.EmployeeReplacementId.HasValue && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen Vertretungen einsetzen.");

        // customer
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.Appointment.CustomerId)
                       ?? throw new BadRequestException("Kunde nicht gefunden.");

        if (user.Id != customer.AssociatedEmployeeId && !user.IsManager)
            throw new ForbiddenException("Nur Manager dürfen Termine für nicht selbst betreute Kunden anlegen.");

        appointment.Customer = new Customer { Id = appointment.CustomerId };

        // employee
        var employee = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.Appointment.EmployeeId)
                       ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");
        appointment.Employee = new Employee { Id = appointment.EmployeeId };

        // employee replacement
        if (appointment.EmployeeReplacementId.HasValue)
        {
            var employeeReplacement = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.Appointment.EmployeeReplacementId!.Value)
                                      ?? throw new NotFoundException("Vertretung nicht gefunden.");
            appointment.EmployeeReplacement = new Employee { Id = appointment.EmployeeReplacementId!.Value };
        }

        appointment = await appointmentRepository.AddAppointmentAsync(appointment);
        return mapper.Map<GetAppointmentDto>(appointment);
    }
}