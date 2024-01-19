namespace Curacaru.Backend.Controllers;

using Application.CQRS.Deployment;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class DeploymentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDeployments()
    {
        var deployments = await mediator.Send(new DeploymentsRequest(CompanyId, AuthId));
        return Ok(deployments);
    }

    [HttpGet("report/{customerId:guid}/{insuranceStatusId:int}")]
    public async Task<IActionResult> GetProofOfDeployment(
        Guid customerId,
        int insuranceStatusId)
    {
        if (!Enum.IsDefined(typeof(InsuranceStatus), insuranceStatusId)) return BadRequest("Insurance status not defined");

        var insuranceStatus = (InsuranceStatus)insuranceStatusId;

        var report = await mediator.Send(new DeploymentReportRequest(CompanyId, AuthId, customerId, insuranceStatus));

        return File(report, "application/pdf");
    }
}