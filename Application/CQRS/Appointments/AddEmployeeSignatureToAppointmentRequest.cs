namespace Curacaru.Backend.Application.CQRS.Appointments;

using Core.DTO.Appointment;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to add an employee's signature to an appointment.</summary>
public class AddEmployeeSignatureToAppointmentRequest(
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

    /// <summary>Gets the signature of the employee.</summary>
    public AddSignatureDto Signature { get; } = signature;
}

internal class AddEmployeeSignatureToAppointmentRequestHandler(
    IEmployeeRepository employeeRepository,
    IAppointmentRepository appointmentRepository,
    IImageService imageService)
    : IRequestHandler<AddEmployeeSignatureToAppointmentRequest>
{
    public async Task Handle(AddEmployeeSignatureToAppointmentRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var appointment = await appointmentRepository.GetAppointmentAsync(request.CompanyId, request.AppointmentId)
                          ?? throw new NotFoundException("Termin nicht gefunden.");

        if ((appointment.EmployeeReplacementId is not null && appointment.EmployeeReplacementId != user!.Id) || appointment.EmployeeId != user.Id)
            throw new BadRequestException("Du darfst diesen Termin nicht unterschreiben");

        if (!string.IsNullOrEmpty(appointment.SignatureEmployee)) throw new BadRequestException("Der Termin wurde bereits von einem Mitarbeiter unterschrieben");

        appointment.SignatureEmployee = imageService.ReduceImage(request.Signature.Signature);
        await appointmentRepository.UpdateAppointmentAsync(appointment);
    }
}