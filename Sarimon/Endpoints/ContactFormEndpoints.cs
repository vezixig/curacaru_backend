namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.ContactForm;
using Core.DTO.ContactForm;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class ContactFormEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        var contactFormEndpoints = app.MapGroup("contact-forms/");
        MapGetContactFormData(contactFormEndpoints);
        MapAddContactInfo(contactFormEndpoints);
        MapGetContactFormForCompany(contactFormEndpoints);
        MapActivateContactForm(contactFormEndpoints);
        MapUpdateContactForm(contactFormEndpoints);
    }

    private void MapActivateContactForm(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "activate",
                async (IMediator Mediator, ClaimsPrincipal principal) => await Mediator.Send(
                    new ActivateContactFormRequest(GetAuthUser(principal))))
            .RequireAuthorization(Policy.Manager)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns the contact form data of the user's company";
                    return generatedOperation;
                });
    }

    private void MapAddContactInfo(IEndpointRouteBuilder app)
    {
        app.MapPost("{id:guid}", async (IMediator mediator, Guid id, [FromBody] AddContactInfoDto data) => await mediator.Send(new AddContactInfoRequest(id, data)));
    }

    private void MapGetContactFormData(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "{id:guid}",
                async (IMediator Mediator, Guid id) => await Mediator.Send(
                    new IsIdValidRequest(id)))
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns the contact form data of a company";
                    return generatedOperation;
                });
    }

    private void MapGetContactFormForCompany(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "",
                async (IMediator Mediator, ClaimsPrincipal principal) => await Mediator.Send(
                    new GetContactFormOfCompanyRequest(GetAuthUser(principal))))
            .RequireAuthorization(Policy.Manager)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Returns the contact form data of the user's company";
                    return generatedOperation;
                });
    }

    private void MapUpdateContactForm(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "",
                async (IMediator Mediator, ClaimsPrincipal principal, [FromBody] UpdateContactFormDto data) => await Mediator.Send(
                    new UpdateContactFormRequest(GetAuthUser(principal), data)))
            .RequireAuthorization(Policy.Manager)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Updates the contact form data of a company";
                    return generatedOperation;
                });
    }
}