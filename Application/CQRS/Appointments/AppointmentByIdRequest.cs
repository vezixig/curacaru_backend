namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to get an appointment by its id.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="appointmentId">The id of the appointment.</param>
public class AppointmentByIdRequest(User user, Guid appointmentId) : IRequest<GetAppointmentDto>
{
    /// <summary>Gets the id of the appointment.</summary>
    public Guid AppointmentId { get; } = appointmentId;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

public class AppointmentByIdRequestHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    : IRequestHandler<AppointmentByIdRequest, GetAppointmentDto>
{
    public async Task<GetAppointmentDto> Handle(AppointmentByIdRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.User.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin wurde nicht gefunden");

        if (appointment.EmployeeReplacementId != request.User.EmployeeId && appointment.EmployeeId != request.User.EmployeeId && !request.User.IsManager)
            throw new ForbiddenException("Du darfst diesen Termin nicht einsehen");

        return mapper.Map<GetAppointmentDto>(appointment);
    }
}