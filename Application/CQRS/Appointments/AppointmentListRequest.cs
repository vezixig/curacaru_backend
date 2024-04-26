namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to get a list of appointments.</summary>
public class AppointmentListRequest(
    User user,
    DateOnly? from,
    DateOnly? to,
    Guid? employeeId,
    Guid? customerId) : IRequest<List<GetAppointmentListEntryDto>>
{
    /// <summary>Gets the id of the customer.</summary>
    public Guid? CustomerId { get; } = customerId;

    /// <summary>Gets the id of the employee.</summary>
    public Guid? EmployeeId { get; } = employeeId;

    /// <summary>Gets the date to start returning appointments for.</summary>
    public DateOnly? From { get; } = from;

    /// <summary>Gets the date to stop returning appointments for.</summary>
    public DateOnly? To { get; } = to;

    public User User { get; } = user;
}

public class AppointmentsRequestHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    : IRequestHandler<AppointmentListRequest, List<GetAppointmentListEntryDto>>
{
    public async Task<List<GetAppointmentListEntryDto>> Handle(AppointmentListRequest request, CancellationToken cancellationToken)
    {
        var employeeId = !request.User.IsManager && request.EmployeeId != request.User.EmployeeId ? request.User.EmployeeId : request.EmployeeId;

        var appointments = await appointmentRepository.GetAppointmentsAsync(request.User.CompanyId, request.From, request.To, employeeId, request.CustomerId);
        return mapper.Map<List<GetAppointmentListEntryDto>>(appointments);
    }
}