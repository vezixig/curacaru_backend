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
                    int year) => await mediator.Send(new WorkingHoursRequest(GetAuthUser(principal), employeeId, month, year)))
            .RequireAuthorization(Policy.Company);

        app.MapGet(
                "work-time/employee/{employeeId:guid}/report",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid employeeId,
                    int year,
                    int month) => await mediator.Send(new WorkingHoursReportRequest(GetAuthUser(principal), employeeId, month, year)))
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
                    var report = await mediator.Send(new WorkingHoursReportPrintRequest(GetAuthUser(principal), employeeId, month, year));
                    return Results.File(report, "application/pdf");
                })
            .RequireAuthorization(Policy.Company);

        app.MapGet(
                "/work-time/list",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    int month,
                    int year,
                    int page,
                    int pageSize = 20) => await mediator.Send(new WorkingHoursListRequest(GetAuthUser(principal), year, month, page, pageSize)))
            .RequireAuthorization(Policy.Company);

        app.MapPost(
                "work-time/sign",
                async (
                        IMediator mediator,
                        ClaimsPrincipal principal,
                        AddWorkingTimeReportSignatureDto dto)
                    => await mediator.Send(new AddWorkingTimeSignatureRequest(GetAuthUser(principal), dto)))
            .RequireAuthorization(Policy.Company);

        app.MapDelete(
                "work-time/report/{reportId:guid}",
                async (
                    IMediator mediator,
                    ClaimsPrincipal principal,
                    Guid reportId) => await mediator.Send(new DeleteWorkingTimeReportRequest(GetCompanyId(principal), reportId))
            )
            .RequireAuthorization(Policy.Manager);
    }
}