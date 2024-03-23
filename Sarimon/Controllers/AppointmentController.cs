namespace Curacaru.Backend.Controllers;

using Application.CQRS.Appointments;
using Core.DTO.Appointment;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class AppointmentController(ISender mediator) : ControllerBase
{
    [HttpPost("new")]
    public async Task<IActionResult> AddAppointment([FromBody] AddAppointmentDto appointment)
    {
        var newAppointment = await mediator.Send(new AddAppointmentRequest(CompanyId, AuthId, appointment));
        return CreatedAtAction(nameof(GetAppointment), new { appointmentId = newAppointment.Id }, newAppointment);
    }

    [HttpPost("{appointmentId:guid}/signature/customer")]
    public async Task<IActionResult> AddCustomerSignature([FromRoute] Guid appointmentId, [FromBody] AddSignatureDto signature)
    {
        await mediator.Send(new AddCustomerSignatureToAppointmentRequest(CompanyId, AuthId, appointmentId, signature));
        return Ok();
    }

    [HttpPost("{appointmentId:guid}/signature/employee")]
    public async Task<IActionResult> AddEmployeeSignature([FromRoute] Guid appointmentId, [FromBody] AddSignatureDto signature)
    {
        await mediator.Send(new AddEmployeeSignatureToAppointmentRequest(CompanyId, AuthId, appointmentId, signature));
        return Ok();
    }

    [HttpDelete("{appointmentId:guid}")]
    public async Task<IActionResult> DeleteAppointment([FromRoute] Guid appointmentId)
    {
        await mediator.Send(new BudgetService(CompanyId, AuthId, appointmentId));
        return NoContent();
    }

    [HttpPost("{appointmentId:guid}/finish")]
    public async Task<IActionResult> FinishAppointment([FromRoute] Guid appointmentId)
    {
        await mediator.Send(new ChangeAppointmentStatusRequest(CompanyId, AuthId, appointmentId, true));
        return NoContent();
    }

    [HttpGet("{appointmentId:guid}")]
    public async Task<IActionResult> GetAppointment([FromRoute] Guid appointmentId)
    {
        var appointment = await mediator.Send(new AppointmentByIdRequest(CompanyId, appointmentId));
        return appointment == null ? NotFound() : Ok(appointment);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] Guid? employeeId,
        [FromQuery] Guid? customerId)
    {
        var appointments = await mediator.Send(new AppointmentsRequest(CompanyId, AuthId, from, to, employeeId, customerId));
        return Ok(appointments);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpPost("{appointmentId:guid}/reopen")]
    public async Task<IActionResult> ReopenAppointment([FromRoute] Guid appointmentId)
    {
        await mediator.Send(new ChangeAppointmentStatusRequest(CompanyId, AuthId, appointmentId, false));
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentDto appointment)
    {
        var updatedAppointment = await mediator.Send(new UpdateAppointmentRequest(CompanyId, AuthId, appointment));
        return Ok(updatedAppointment);
    }
}