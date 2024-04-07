namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.DTO.Appointment;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to add a customer's signature to an appointment.</summary>
public class AddCustomerSignatureToAppointmentRequest(
    Guid companyId,
    string AuthId,
    Guid appointmentId,
    AddSignatureDto signature) : IRequest
{
    /// <summary>Gets the id of the appointment.</summary>
    public Guid AppointmentId { get; } = appointmentId;

    /// <summary>Gets the id of the user.</summary>
    public string AuthId { get; } = AuthId;

    /// <summary>Gets the id of the company.</summary>
    public Guid CompanyId { get; } = companyId;

    /// <summary>Gets the signature of the customer.</summary>
    public AddSignatureDto Signature { get; } = signature;
}

internal class AddCustomerSignatureToAppointmentRequestHandler(
    IEmployeeRepository employeeRepository,
    IAppointmentRepository appointmentRepository,
    IImageService imageService)
    : IRequestHandler<AddCustomerSignatureToAppointmentRequest>
{
    public async Task Handle(AddCustomerSignatureToAppointmentRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if (appointment.Date > DateOnly.FromDateTime(DateTime.Now))
            throw new BadRequestException("Der Termin liegt in der Zukunft und kann noch nicht unterschrieben werden.");

        if (appointment.EmployeeId != user!.Id && appointment.EmployeeReplacementId != user.Id)
            throw new BadRequestException("Du darfst diesen Termin nicht unterschreiben lassen");

        if (!string.IsNullOrEmpty(appointment.SignatureCustomer)) throw new BadRequestException("Der Termin wurde bereits von einem Kunden unterschrieben");

        appointment.SignatureCustomer = imageService.ReduceImage(request.Signature.Signature);
        await appointmentRepository.UpdateAppointmentAsync(appointment);
    }
}