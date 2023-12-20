namespace Curacaru.Backend.Controllers;

using Application.CQRS.Appointments;
using Core.DTO;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class AppointmentController(ISender mediator) : ControllerBase
{
    [Authorize(Policy = Policy.Manager)]
    [HttpPost("new")]
    public async Task<IActionResult> AddAppointment([FromBody] AddAppointmentDto appointment)
    {
        var newAppointment = await mediator.Send(new AddAppointmentRequest(CompanyId, appointment));
        return CreatedAtAction(nameof(GetAppointment), new { appointmentId = newAppointment.Id }, newAppointment);
    }

    [HttpGet("{appointmentId}")]
    public async Task<IActionResult> GetAppointment([FromRoute] string appointmentId)
    {
        if (!Guid.TryParse(appointmentId, out var appointmentGuid)) return BadRequest("Appointment Id invalid.");

        var appointment = await mediator.Send(new AppointmentByIdRequest(CompanyId, appointmentGuid));
        return appointment == null ? NotFound() : Ok(appointment);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] Guid? employeeId,
        [FromQuery] Guid? customerId)
    {
        var appointments = await mediator.Send(new AppointmentsRequest(CompanyId, from, to, employeeId, customerId));
        return Ok(appointments);
    }
}