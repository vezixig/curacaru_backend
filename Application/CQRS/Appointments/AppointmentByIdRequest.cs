namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.DTO;
using MediatR;

public class AppointmentByIdRequest(Guid companyId, Guid appointmentId) : IRequest<GetAppointmentDto?>
{
    public Guid AppointmentId { get; } = appointmentId;

    public Guid CompanyId { get; } = companyId;
}