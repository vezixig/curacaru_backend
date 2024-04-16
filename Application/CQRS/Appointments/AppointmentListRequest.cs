namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO.Appointment;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to get a list of appointments.</summary>
public class AppointmentListRequest(
    Guid companyId,
    string authId,
    DateOnly? from,
    DateOnly? to,
    Guid? employeeId,
    Guid? customerId) : IRequest<List<GetAppointmentListEntryDto>>
{
    /// <summary>Gets the id of the user.</summary>
    public string AuthId { get; } = authId;

    /// <summary>Gets the id of the company.</summary>
    public Guid CompanyId { get; } = companyId;

    /// <summary>Gets the id of the customer.</summary>
    public Guid? CustomerId { get; } = customerId;

    /// <summary>Gets the id of the employee.</summary>
    public Guid? EmployeeId { get; } = employeeId;

    /// <summary>Gets the date to start returning appointments for.</summary>
    public DateOnly? From { get; } = from;

    /// <summary>Gets the date to stop returning appointments for.</summary>
    public DateOnly? To { get; } = to;
}

public class AppointmentsRequestHandler(IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<AppointmentListRequest, List<GetAppointmentListEntryDto>>
{
    public async Task<List<GetAppointmentListEntryDto>> Handle(AppointmentListRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        var employeeId = !user!.IsManager && request.EmployeeId != user.Id ? user.Id : request.EmployeeId;

        var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, request.From, request.To, employeeId, request.CustomerId);
        return mapper.Map<List<GetAppointmentListEntryDto>>(appointments);
    }
}