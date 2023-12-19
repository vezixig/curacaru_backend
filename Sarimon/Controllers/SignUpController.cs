namespace Curacaru.Backend.Controllers;

using Application.CQRS.SignUp;
using Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SignUpController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        var employee = await mediator.Send(new SignUpCommand(signUpDto, AuthId));
        return Ok(employee);
    }
}