namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.Documents;
using Core.DTO.AssignmentDeclaration;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>Endpoints concerning documents.</summary>
public class DocumentEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet(
                "/documents/assignment-declarations/{year:int}",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        [FromRoute] int year,
                        [FromQuery] Guid? employeeId,
                        [FromQuery] Guid? customerId)
                    => await mediator.Send(new AssignmentDeclarationsRequest(GetCompanyId(principal), GetAuthId(principal), year, employeeId, customerId)))
            .RequireAuthorization(Policy.Company)
            .Produces<List<GetAssignmentDeclarationListEntryDto>>()
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Gets the list of assignment declarations for the given year and filters optional by employee and/or customer.";
                    return generatedOperation;
                });

        app.MapPost(
                "/documents/assignment-declarations",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        AddAssignmentDeclarationDto data) =>
                    await mediator.Send(new AddAssignmentDeclarationRequest(GetCompanyId(principal), GetAuthId(principal), data)))
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Adds a new and signed assignment declaration.";
                    return generatedOperation;
                });

        app.MapGet(
                "/documents/assignment-declarations/{year:int}/{customerId:guid}",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid customerId,
                    int year) =>
                {
                    var document = await mediator.Send(new AssignmentDeclarationRequest(GetCompanyId(principal), GetAuthId(principal), customerId, year));
                    return Results.File(document, "application/pdf");
                })
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns a customer's assignment declaration for the given year.";
                    return generatedOperation;
                });
    }
}