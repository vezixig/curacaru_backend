namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to delete an appointment.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="authId">The auth id of the user.</param>
/// <param name="appointmentId">The appointment id.</param>
public class DeleteAppointmentRequest(Guid companyId, string authId, Guid appointmentId) : IRequest
{
    public Guid AppointmentId { get; } = appointmentId;

    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class DeleteAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<DeleteAppointmentRequest>
{
    public async Task Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != appointment.EmployeeId && !user.IsManager) throw new ForbiddenException("Nur Manager dürfen fremde Termine löschen.");

        await appointmentRepository.DeleteAppointmentAsync(appointment);
    }
}