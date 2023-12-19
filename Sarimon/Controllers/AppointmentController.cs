namespace Curacaru.Backend.Controllers;

using Application.CQRS.Appointments;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class AppointmentController(ISender mediator) : ControllerBase
{
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