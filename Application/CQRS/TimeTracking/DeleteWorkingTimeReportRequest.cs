namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to delete a working time report</summary>
public class DeleteWorkingTimeReportRequest(Guid companyId, Guid reportId) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public Guid ReportId { get; } = reportId;
}

internal class DeleteWorkingTimeReportRequestHandler(IWorkingTimeRepository workingTimeRepository)
    : IRequestHandler<DeleteWorkingTimeReportRequest>
{
    public async Task Handle(DeleteWorkingTimeReportRequest request, CancellationToken cancellationToken)
    {
        var report = await workingTimeRepository.GetWorkingTimeReportByIdAsync(request.CompanyId, request.ReportId)
                     ?? throw new BadRequestException("Die Einsatzzeiterfassung konnte nicht gefunden werden.");

        await workingTimeRepository.DeleteWorkingTimeReportAsync(report!);
    }
}