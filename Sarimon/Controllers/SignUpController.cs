namespace Curacaru.Backend.Controllers;

using System.Security.Claims;
using Application.CQRS.SignUp;
using Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SignUpController : ControllerBase
{
    private readonly ILogger<SignUpController> _logger;

    private readonly IMediator _mediator;

    public SignUpController(ILogger<SignUpController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        var authId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var employee = await _mediator.Send(new SignUpCommand(signUpDto, authId));
        return Ok(employee);
    }
}