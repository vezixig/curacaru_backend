namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.Payment;
using Core.Enums;
using MediatR;

public class PaymentEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup("payment");

        group.MapGet(
            "status",
            async (IMediator mediator, ClaimsPrincipal principal) =>
                await mediator.Send(new GetSubscriptionStatusRequest(GetCompanyId(principal))));

        group.MapPost(
            "start-session",
            async (SubscriptionType type, IMediator mediator, ClaimsPrincipal principal) =>
            {
                var url = await mediator.Send(new StartPaymentSessionRequest(GetCompanyId(principal), type));
                return new { url };
            });

        group.MapPost(
            "finish-session",
            async (string sessionId, IMediator mediator, ClaimsPrincipal principal) =>
            {
                await mediator.Send(new FinishPaymentSessionRequest(GetCompanyId(principal), sessionId));
                return Results.NoContent();
            });

        group.MapPost(
            "subscription/renew",
            async (IMediator mediator, ClaimsPrincipal principal) =>
            {
                await mediator.Send(new RenewSubscriptionRequest(GetCompanyId(principal)));
                return Results.NoContent();
            });

        group.MapDelete(
            "subscription",
            async (IMediator mediator, ClaimsPrincipal principal) =>
            {
                await mediator.Send(new DeleteSubscriptionRequest(GetCompanyId(principal)));
                return Results.NoContent();
            });

        group.MapPost(
            "subscription/cancel",
            async (IMediator mediator, ClaimsPrincipal principal) =>
            {
                await mediator.Send(new CancelSubscriptionRequest(GetCompanyId(principal)));
                return Results.NoContent();
            });

        group.MapPost(
            "subscription/upgrade",
            async (SubscriptionType type, IMediator mediator, ClaimsPrincipal principal) =>
            {
                await mediator.Send(new UpgradeSubscriptionRequest(GetCompanyId(principal), type));
                return Results.NoContent();
            });

        group.MapPost(
            "subscription/downgrade",
            async (SubscriptionType type, IMediator mediator, ClaimsPrincipal principal) =>
            {
                await mediator.Send(new DowngradeSubscriptionRequest(GetCompanyId(principal), type));
                return Results.NoContent();
            });
    }
}