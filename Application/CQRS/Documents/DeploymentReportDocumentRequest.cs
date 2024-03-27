namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request for a deployment report.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="authId">The auth id used to check if user can view customer.</param>
/// <param name="customerId">The customer id the report is for.</param>
public class DeploymentReportDocumentRequest(
    Guid companyId,
    string authId,
    Guid customerId,
    ClearanceType clearanceType,
    int year,
    int month) : IRequest<byte[]>
{
    public string AuthId { get; } = authId;

    public ClearanceType ClearanceType { get; } = clearanceType;

    public Guid CompanyId { get; } = companyId;

    public Guid CustomerId { get; } = customerId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class DeploymentReportDocumentRequestHandler(
    ICustomerRepository customerRepository,
    ICompanyRepository companyRepository,
    IDocumentRepository documentRepository,
    IEmployeeRepository employeeRepository,
    IReportService reportService)
    : IRequestHandler<DeploymentReportDocumentRequest, byte[]>
{
    public async Task<byte[]> Handle(DeploymentReportDocumentRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var report = await documentRepository.GetDeploymentReportsAsync(
            request.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month,
            request.ClearanceType,
            true);
        if (report.Count != 1) throw new BadRequestException("Einsatznachweis existiert nicht.");

        if (!user!.IsManager && !report[0].Appointments.Exists(o => o.EmployeeId == user.Id || o.EmployeeReplacementId == user.Id))
            throw new ForbiddenException("Du darfst diesen Einsatznachweis nicht herunterladen.");

        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);

        var reportDocument = reportService.CreateDeploymentReport(company!, report[0]);
        return reportDocument;
    }
}