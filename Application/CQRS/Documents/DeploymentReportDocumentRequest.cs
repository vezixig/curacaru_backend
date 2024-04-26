namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request for a deployment report.</summary>
/// <param name="customerId">The customer id the report is for.</param>
public class DeploymentReportDocumentRequest(
    User user,
    Guid customerId,
    ClearanceType clearanceType,
    int year,
    int month) : IRequest<byte[]>
{
    public ClearanceType ClearanceType { get; } = clearanceType;

    public Guid CustomerId { get; } = customerId;

    public int Month { get; } = month;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class DeploymentReportDocumentRequestHandler(
    ICompanyRepository companyRepository,
    IDocumentRepository documentRepository,
    IReportService reportService)
    : IRequestHandler<DeploymentReportDocumentRequest, byte[]>
{
    public async Task<byte[]> Handle(DeploymentReportDocumentRequest request, CancellationToken cancellationToken)
    {
        var report = await documentRepository.GetDeploymentReportsAsync(
            request.User.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month,
            request.ClearanceType,
            true);
        if (report.Count != 1) throw new BadRequestException("Einsatznachweis existiert nicht.");

        if (!request.User.IsManager
            && !report[0].Appointments.Exists(o => o.EmployeeId == request.User.EmployeeId || o.EmployeeReplacementId == request.User.EmployeeId))
            throw new ForbiddenException("Du darfst diesen Einsatznachweis nicht herunterladen.");

        var company = await companyRepository.GetCompanyByIdAsync(request.User.CompanyId);

        var reportDocument = reportService.CreateDeploymentReport(company!, report[0]);
        return reportDocument;
    }
}