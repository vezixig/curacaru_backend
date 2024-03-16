namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.TimeTracking;
using Core.DTO.TimeTracker;
using Core.Enums;
using MediatR;

public class TimeTrackerEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapGet(
                "/work-time/employee/{employeeId:guid}",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid employeeId,
                    int month,
                    int year) => await mediator.Send(new WorkingHoursRequest(GetCompanyId(principal), employeeId, GetAuthId(principal), month, year)))
            .RequireAuthorization(Policy.Company);

        app.MapGet(
                "work-time/employee/{employeeId:guid}/report",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid employeeId,
                    int year,
                    int month) => await mediator.Send(new WorkingHoursReportRequest(GetCompanyId(principal), GetAuthId(principal), employeeId, month, year)))
            .RequireAuthorization(Policy.Company);

        app.MapGet(
                "/work-time/employee/{employeeId:guid}/report/{year:int}/{month:int}/print",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid employeeId,
                    int year,
                    int month) =>
                {
                    var report = await mediator.Send(new WorkingHoursReportPrintRequest(GetCompanyId(principal), GetAuthId(principal), employeeId, month, year));
                    return Results.File(report, "application/pdf");
                })
            .RequireAuthorization(Policy.Company);

        app.MapGet(
                "/work-time/list",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    int month,
                    int year) => await mediator.Send(new WorkingHoursListRequest(GetCompanyId(principal), GetAuthId(principal), year, month)))
            .RequireAuthorization(Policy.Company);

        app.MapPost(
                "work-time/sign",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        AddWorkingTimeReportSignatureDto dto)
                    => await mediator.Send(new AddWorkingTimeSignatureRequest(GetCompanyId(principal), GetAuthId(principal), dto)))
            .RequireAuthorization(Policy.Company);
    }
}