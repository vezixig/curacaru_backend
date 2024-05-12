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
        var newAppointment = await mediator.Send(new AddAppointmentRequest(AuthUser, appointment));
        return CreatedAtAction(nameof(GetAppointment), new { appointmentId = newAppointment.Id }, newAppointment);
    }

    [HttpPost("{appointmentId:guid}/signature/customer")]
    public async Task<IActionResult> AddCustomerSignature([FromRoute] Guid appointmentId, [FromBody] AddSignatureDto signature)
    {
        await mediator.Send(new AddCustomerSignatureToAppointmentRequest(AuthUser, appointmentId, signature));
        return NoContent();
    }

    [HttpPost("{appointmentId:guid}/signature/employee")]
    public async Task<IActionResult> AddEmployeeSignature([FromRoute] Guid appointmentId, [FromBody] AddSignatureDto signature)
    {
        await mediator.Send(new AddEmployeeSignatureToAppointmentRequest(AuthUser, appointmentId, signature));
        return Ok();
    }

    [HttpDelete("{appointmentId:guid}")]
    public async Task<IActionResult> DeleteAppointment([FromRoute] Guid appointmentId)
    {
        await mediator.Send(new DeleteAppointmentRequest(AuthUser, appointmentId));
        return NoContent();
    }

    [HttpPost("{appointmentId:guid}/finish")]
    public async Task<IActionResult> FinishAppointment([FromRoute] Guid appointmentId)
    {
        await mediator.Send(new ChangeAppointmentStatusRequest(AuthUser, appointmentId, true));
        return NoContent();
    }

    [HttpGet("{appointmentId:guid}")]
    public async Task<IActionResult> GetAppointment([FromRoute] Guid appointmentId)
    {
        var appointment = await mediator.Send(new AppointmentByIdRequest(AuthUser, appointmentId));
        return Ok(appointment);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] Guid? employeeId,
        [FromQuery] Guid? customerId,
        [FromQuery] int page,
        [FromQuery] int pageSize = 20)
    {
        var appointments = await mediator.Send(new AppointmentListRequest(AuthUser, from, to, page, pageSize, employeeId, customerId));
        return Ok(appointments);
    }

    [Authorize(Policy = Policy.Manager)]
    [HttpPost("{appointmentId:guid}/reopen")]
    public async Task<IActionResult> ReopenAppointment([FromRoute] Guid appointmentId)
    {
        await mediator.Send(new ChangeAppointmentStatusRequest(AuthUser, appointmentId, false));
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentDto appointment)
    {
        var updatedAppointment = await mediator.Send(new UpdateAppointmentRequest(AuthUser, appointment));
        return Ok(updatedAppointment);
    }
}