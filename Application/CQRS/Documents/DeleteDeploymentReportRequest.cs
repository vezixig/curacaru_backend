namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class DeleteDeploymentReportRequest(Guid companyId, Guid reportId) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public Guid ReportId { get; } = reportId;
}

internal class DeleteDeploymentReportRequestHandler(IDocumentRepository documentRepository)
    : IRequestHandler<DeleteDeploymentReportRequest>
{
    public async Task Handle(DeleteDeploymentReportRequest request, CancellationToken cancellationToken)
    {
        var report = await documentRepository.GetDeploymentReportByIdAsync(request.CompanyId, request.ReportId)
                     ?? throw new NotFoundException("Der Einsatznachweis existiert nicht");

        report.Appointments.ForEach(o => o.DeploymentReportId = null);
        await documentRepository.DeleteDeploymentReportAsync(report);
    }
}