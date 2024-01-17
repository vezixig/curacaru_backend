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
    [HttpGet("{year:int}/{month:int}")]
    public async Task<IActionResult> GetDeployments(int year, int month)
    {
        var deployments = await mediator.Send(new DeploymentsRequest(CompanyId, AuthId, year, month));
        return Ok(deployments);
    }

    [HttpGet("report/{year:int}/{month:int}/{customerId:guid}/{insuranceStatusId:int}")]
    public async Task<IActionResult> GetProofOfDeployment(
        int year,
        int month,
        Guid customerId,
        int insuranceStatusId)
    {
        if (!Enum.IsDefined(typeof(InsuranceStatus), insuranceStatusId)) return BadRequest("Insurance status not defined");

        var insuranceStatus = (InsuranceStatus)insuranceStatusId;

        var report = await mediator.Send(new DeploymentReportRequest(CompanyId, AuthId, year, month, customerId, insuranceStatus));

        // var service = new ReportService();
        //var document = service.CreateDeploymentReport(InsuranceStatus.Statutory);

        //BlobContent blobContent = new BlobContent(byteArray, "application/octet-stream");
        //Blob blob = new Blob(blobContent);

        return File(report, "application/pdf");
    }
}