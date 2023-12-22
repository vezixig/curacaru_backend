namespace Curacaru.Backend.Application.CQRS.Appointments;

using AutoMapper;
using Core.DTO;
using Infrastructure.Repositories;
using MediatR;

public class AppointmentByIdRequest(Guid companyId, Guid appointmentId) : IRequest<GetAppointmentDto?>
{
    public Guid AppointmentId { get; } = appointmentId;

    public Guid CompanyId { get; } = companyId;
}

public class AppointmentByIdRequestHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
    : IRequestHandler<AppointmentByIdRequest, GetAppointmentDto?>
{
    public async Task<GetAppointmentDto?> Handle(AppointmentByIdRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId);
        return mapper.Map<GetAppointmentDto?>(appointment);
    }
}