namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO;
using Core.DTO.Appointment;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to get a list of appointments.</summary>
public class AppointmentListRequest(
    User user,
    DateOnly? from,
    DateOnly? to,
    int page,
    int pageSize,
    Guid? employeeId,
    Guid? customerId) : IRequest<PageDto<GetAppointmentListEntryDto>>
{
    /// <summary>Gets the id of the customer.</summary>
    public Guid? CustomerId { get; } = customerId;

    /// <summary>Gets the id of the employee.</summary>
    public Guid? EmployeeId { get; } = employeeId;

    /// <summary>Gets the date to start returning appointments for.</summary>
    public DateOnly? From { get; } = from;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    /// <summary>Gets the date to stop returning appointments for.</summary>
    public DateOnly? To { get; } = to;

    public User User { get; } = user;
}

public class AppointmentsRequestHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    : IRequestHandler<AppointmentListRequest, PageDto<GetAppointmentListEntryDto>>
{
    public async Task<PageDto<GetAppointmentListEntryDto>> Handle(AppointmentListRequest request, CancellationToken cancellationToken)
    {
        var employeeId = !request.User.IsManager && request.EmployeeId != request.User.EmployeeId ? request.User.EmployeeId : request.EmployeeId;

        double appointmentCount = await appointmentRepository.GetAppointmentCountAsync(
            request.User.CompanyId,
            request.From,
            request.To,
            employeeId,
            request.CustomerId);
        var appointments = await appointmentRepository.GetAppointmentsAsync(
            request.User.CompanyId,
            request.From,
            request.To,
            employeeId,
            request.CustomerId,
            request.Page,
            request.PageSize);
        var pageCount = (int)Math.Ceiling(appointmentCount / request.PageSize);
        return new(mapper.Map<List<GetAppointmentListEntryDto>>(appointments), request.Page, pageCount);
    }
}