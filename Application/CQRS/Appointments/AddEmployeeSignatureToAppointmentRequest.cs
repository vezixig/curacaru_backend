namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.DTO.Appointment;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to add an employee's signature to an appointment.</summary>
public class AddEmployeeSignatureToAppointmentRequest(
    User user,
    Guid appointmentId,
    AddSignatureDto signature) : IRequest
{
    /// <summary>Gets the id of the appointment.</summary>
    public Guid AppointmentId { get; } = appointmentId;

    /// <summary>Gets the signature of the employee.</summary>
    public AddSignatureDto Signature { get; } = signature;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class AddEmployeeSignatureToAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IImageService imageService)
    : IRequestHandler<AddEmployeeSignatureToAppointmentRequest>
{
    public async Task Handle(AddEmployeeSignatureToAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.User.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if (appointment.Date > DateOnly.FromDateTime(DateTime.Now))
            throw new BadRequestException("Der Termin liegt in der Zukunft und kann noch nicht unterschrieben werden.");

        if ((appointment.EmployeeReplacementId is not null && appointment.EmployeeReplacementId != request.User.EmployeeId)
            || (appointment.EmployeeReplacementId is null && appointment.EmployeeId != request.User.EmployeeId))
            throw new BadRequestException("Du darfst diesen Termin nicht unterschreiben");

        if (!string.IsNullOrEmpty(appointment.SignatureEmployee)) throw new BadRequestException("Der Termin wurde bereits von einem Mitarbeiter unterschrieben");

        appointment.SignatureEmployee = imageService.ReduceImage(request.Signature.Signature);
        await appointmentRepository.UpdateAppointmentAsync(appointment);
    }
}