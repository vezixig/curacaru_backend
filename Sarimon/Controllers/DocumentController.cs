namespace Curacaru.Backend.Controllers;

using Application.CQRS.Documents;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Policy = Policy.Company)]
[ApiController]
[Route("[controller]")]
public class DocumentController(IMediator mediator) : ControllerBase
{
    [HttpGet("assignment-declaration/{customerId:guid}/{year:int}")]
    public async Task<IActionResult> GetDeclarationOfAssignment(Guid customerId, int year)
    {
        var report = await mediator.Send(new AssignmentDeclarationRequest(CompanyId, AuthId, customerId, year));
        return File(report, "application/pdf");
    }

    [HttpGet("deployment/{customerId:guid}/{insuranceStatusId:int}")]
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