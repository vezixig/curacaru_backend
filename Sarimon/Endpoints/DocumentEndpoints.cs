namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.Documents;
using Core.DTO.AssignmentDeclaration;
using Core.DTO.Documents;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>Endpoints concerning documents.</summary>
public class DocumentEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        var deploymentReportEndpoints = app.MapGroup("/documents/deployment-reports");

        // Deployment reports
        MapDeleteDeploymentReport(deploymentReportEndpoints);
        MapGetDeploymentReportList(deploymentReportEndpoints);
        MapGetDeploymentReport(deploymentReportEndpoints);
        MapGetDeploymentReportDocument(app);
        MapPostDeploymentReport(deploymentReportEndpoints);

        // Assignment declarations
        MapDeleteAssignmentDeclaration(app);
        MapGetAssignmentDeclarationsList(app);
        MapGetAssignmentDeclarationDocument(app);
        MapPostAssignmentDeclaration(app);
    }

    private void MapDeleteAssignmentDeclaration(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/documents/assignment-declarations/{id:guid}",
                async (IMediator Mediator, ClaimsPrincipal principal, Guid id) => await Mediator.Send(
                    new DeleteAssignmentDeclarationRequest(GetCompanyId(principal), id)))
            .RequireAuthorization(Policy.Manager)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Deletes an assignment declaration by its id.";
                    return generatedOperation;
                });
    }

    private void MapDeleteDeploymentReport(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/{id:guid}",
                async (IMediator Mediator, ClaimsPrincipal principal, Guid id) => await Mediator.Send(
                    new DeleteDeploymentReportRequest(GetCompanyId(principal), id)))
            .RequireAuthorization(Policy.Manager)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Deletes a deployment report by its id.";
                    return generatedOperation;
                });
    }

    private void MapGetAssignmentDeclarationDocument(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/documents/assignment-declarations/{year:int}/{customerId:guid}",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid customerId,
                    int year) =>
                {
                    var document = await mediator.Send(new AssignmentDeclarationRequest(GetAuthUser(principal), customerId, year));
                    return Results.File(document, "application/pdf");
                })
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns a customer's assignment declaration for the given year.";
                    return generatedOperation;
                });
    }

    private void MapGetAssignmentDeclarationsList(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/documents/assignment-declarations/{year:int}",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        [FromRoute] int year,
                        [FromQuery] Guid? employeeId,
                        [FromQuery] Guid? customerId)
                    => await mediator.Send(new AssignmentDeclarationsRequest(GetAuthUser(principal), year, employeeId, customerId)))
            .RequireAuthorization(Policy.Company)
            .Produces<List<GetAssignmentDeclarationListEntryDto>>()
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Gets the list of assignment declarations for the given year and filters optional by employee and/or customer.";
                    return generatedOperation;
                });
    }

    private void MapGetDeploymentReport(IEndpointRouteBuilder deploymentReportEndpoints)
    {
        deploymentReportEndpoints.MapGet(
                "{year:int}/{month:int}/{customerId:guid}/{clearanceType:int}",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        int year,
                        int month,
                        Guid customerId,
                        int clearanceType) =>
                    await mediator.Send(
                        new DeploymentReportRequest(GetAuthUser(principal), customerId, year, month, (ClearanceType)clearanceType)))
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns the deployment report matching the filters or null if none is found.";
                    return generatedOperation;
                });
    }

    private void MapGetDeploymentReportDocument(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/documents/deployment-reports/{year:int}/{month:int}/{customerId:guid}/{clearanceType:int}/document",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid customerId,
                    int clearanceType,
                    int year,
                    int month) =>
                {
                    var document = await mediator.Send(
                        new DeploymentReportDocumentRequest(GetAuthUser(principal), customerId, (ClearanceType)clearanceType, year, month));
                    return Results.File(document, "application/pdf");
                })
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns the deployment report document for the given customer, clearance type, year and month.";
                    return generatedOperation;
                });
    }

    private void MapGetDeploymentReportList(IEndpointRouteBuilder deploymentReportEndpoints)
    {
        deploymentReportEndpoints.MapGet(
                "{year:int}/{month:int}",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        [FromRoute] int year,
                        [FromRoute] int month,
                        [FromQuery] Guid? customerId,
                        [FromQuery] Guid? employeeId) =>
                    await mediator.Send(
                        new DeploymentReportsRequest(GetAuthUser(principal), year, month, customerId, employeeId)
                    ))
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns the deployment reports matching the given filters.";
                    return generatedOperation;
                });
    }

    private void MapPostAssignmentDeclaration(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/documents/assignment-declarations",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        AddAssignmentDeclarationDto data) =>
                    await mediator.Send(new AddAssignmentDeclarationRequest(GetAuthUser(principal), data)))
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Adds a new and signed assignment declaration.";
                    return generatedOperation;
                });
    }

    private void MapPostDeploymentReport(IEndpointRouteBuilder deploymentReportEndpoints)
    {
        deploymentReportEndpoints.MapPost(
                "",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        AddDeploymentReportDto data) =>
                    await mediator.Send(
                        new AddDeploymentReportRequest(GetAuthUser(principal), data)))
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Adds a new deployment report.";
                    return generatedOperation;
                });
    }
}