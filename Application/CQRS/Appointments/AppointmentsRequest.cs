namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO;
using Infrastructure.Repositories;
using MediatR;

public class AppointmentsRequest(
    Guid companyId,
    DateOnly? from,
    DateOnly? to,
    Guid? employeeId,
    Guid? customerId) : IRequest<List<GetAppointmentListEntryDto>>
{
    public Guid CompanyId { get; } = companyId;

    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public DateOnly? From { get; } = from;

    public DateOnly? To { get; } = to;
}

public class AppointmentsRequestHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    : IRequestHandler<AppointmentsRequest, List<GetAppointmentListEntryDto>>
{
    public async Task<List<GetAppointmentListEntryDto>> Handle(AppointmentsRequest request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, request.From, request.To, request.EmployeeId, request.CustomerId);
        return mapper.Map<List<GetAppointmentListEntryDto>>(appointments);
    }
}