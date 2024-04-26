namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.DTO.Appointment;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to add a customer's signature to an appointment.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="appointmentId">The id of the appointment.</param>
/// <param name="signature">The customer's signature.</param>
public class AddCustomerSignatureToAppointmentRequest(
    User user,
    Guid appointmentId,
    AddSignatureDto signature) : IRequest
{
    /// <summary>Gets the id of the appointment.</summary>
    public Guid AppointmentId { get; } = appointmentId;

    /// <summary>Gets the signature of the customer.</summary>
    public AddSignatureDto Signature { get; } = signature;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class AddCustomerSignatureToAppointmentRequestHandler(
    IAppointmentRepository appointmentRepository,
    IImageService imageService)
    : IRequestHandler<AddCustomerSignatureToAppointmentRequest>
{
    public async Task Handle(AddCustomerSignatureToAppointmentRequest request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetAppointmentAsync(request.User.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if (appointment.Date > DateOnly.FromDateTime(DateTime.Now))
            throw new BadRequestException("Der Termin liegt in der Zukunft und kann noch nicht unterschrieben werden.");

        if (appointment.EmployeeId != request.User.EmployeeId && appointment.EmployeeReplacementId != request.User.EmployeeId)
            throw new BadRequestException("Du darfst diesen Termin nicht unterschreiben lassen");

        if (!string.IsNullOrEmpty(appointment.SignatureCustomer)) throw new BadRequestException("Der Termin wurde bereits von einem Kunden unterschrieben");

        appointment.SignatureCustomer = imageService.ReduceImage(request.Signature.Signature);
        await appointmentRepository.UpdateAppointmentAsync(appointment);
    }
}