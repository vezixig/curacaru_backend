namespace Curacaru.Backend.Endpoints;

using System.Security.Claims;
using Application.CQRS.Appointments;
using Core.Enums;
using MediatR;

public class AppointmentEndpoints : EndpointsBase, IEndpoints
{
    public void MapEndpoints(WebApplication app)
    {
        var deploymentReportEndpoints = app.MapGroup("/appointments");

        MapGetIsBlockingAppointment(deploymentReportEndpoints);
    }

    private void MapGetIsBlockingAppointment(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/is-blocking",
                async (
                    IMediator Mediator,
                    ClaimsPrincipal principal,
                    Guid employeeId,
                    DateOnly date,
                    TimeOnly start,
                    TimeOnly end,
                    Guid? appointmentId) => await Mediator.Send(
                    new IsBlockingAppointmentRequest(GetCompanyId(principal), employeeId, date, start, end, appointmentId)))
            .RequireAuthorization(Policy.Company)
            .WithOpenApi(
                generatedOperation =>
                {
                    generatedOperation.Description = "Checks if an appointment would block another appointment.";
                    return generatedOperation;
                });
    }
}