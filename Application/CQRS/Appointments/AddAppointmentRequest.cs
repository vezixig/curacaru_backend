namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO;
using Core.Entities;
using Infrastructure.Repositories;
using MediatR;

public class AddAppointmentRequest(Guid companyId, AddAppointmentDto appointment) : IRequest<GetAppointmentDto>
{
    public AddAppointmentDto Appointment { get; } = appointment;

    public Guid CompanyId { get; } = companyId;
}

internal class AddAppointmentRequestHandler(IAppointmentRepository appointmentRepository, IMapper mapper) : IRequestHandler<AddAppointmentRequest, GetAppointmentDto>
{
    public async Task<GetAppointmentDto> Handle(AddAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = mapper.Map<Appointment>(request.Appointment);
        appointment.CompanyId = request.CompanyId;
        appointment.Customer = new Customer { Id = appointment.CustomerId };
        appointment.Employee = new Employee { Id = appointment.EmployeeId };
        if (appointment.EmployeeReplacementId.HasValue) appointment.EmployeeReplacement = new Employee { Id = appointment.EmployeeReplacementId.Value };

        appointment = await appointmentRepository.AddAppointmentAsync(appointment);
        return mapper.Map<GetAppointmentDto>(appointment);
    }
}