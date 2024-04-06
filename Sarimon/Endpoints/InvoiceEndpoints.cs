namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.Invoices;
using Core.DTO.Invoice;
using Core.Enums;
using MediatR;

public class InvoiceEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet(
                "/invoices/{year:int}/{month:int}",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    int year,
                    int month) => await mediator.Send(new InvoiceListRequest(GetCompanyId(principal), year, month)))
            .RequireAuthorization(Policy.Manager);

        app.MapGet(
                "/invoices/next-number",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal) => await mediator.Send(new NextInvoiceNumberRequest(GetCompanyId(principal))))
            .RequireAuthorization(Policy.Manager);

        app.MapGet(
                "invoices/{invoiceId:guid}/document",
                async (IMediator mediator, ClaimsPrincipal principal, Guid invoiceId)
                    =>
                {
                    var document = await mediator.Send(new InvoiceDocumentRequest(GetCompanyId(principal), invoiceId));
                    return Results.File(document, "application/pdf");
                })
            .RequireAuthorization(Policy.Manager);

        app.MapDelete(
                "invoices/{invoiceId:guid}",
                async (IMediator mediator, ClaimsPrincipal principal, Guid invoiceId) =>
                {
                    await mediator.Send(new DeleteInvoiceRequest(GetCompanyId(principal), invoiceId));
                    return Results.NoContent();
                })
            .RequireAuthorization(Policy.Manager);

        app.MapPost(
                "/invoices",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    AddInvoiceDto invoice) =>
                {
                    await mediator.Send(new AddInvoiceRequest(GetCompanyId(principal), GetUserId(principal), invoice));
                    return Results.NoContent();
                })
            .RequireAuthorization(Policy.Manager)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Adds an new invoice";
                    return generatedOperation;
                });
    }
}