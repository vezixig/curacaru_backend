namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class FinishAppointmentRequest(Guid companyId, string authId, Guid appointmentId) : IRequest<GetAppointmentListEntryDto>
{
    public Guid AppointmentId { get; } = appointmentId;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class FinishAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<FinishAppointmentRequest, GetAppointmentListEntryDto>
{
    public async Task<GetAppointmentListEntryDto> Handle(FinishAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != appointment.EmployeeId && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen fremde Termine löschen.");

        appointment.IsDone = true;
        appointment.Employee = new Employee { Id = appointment.EmployeeId };
        appointment.EmployeeReplacement = appointment.EmployeeReplacementId.HasValue ? new Employee { Id = appointment.EmployeeReplacementId!.Value } : null;
        appointment.Customer = new Customer { Id = appointment.CustomerId };

        await appointmentRepository.UpdateAppointmentAsync(appointment);

        return mapper.Map<GetAppointmentListEntryDto>(appointment);
    }
}