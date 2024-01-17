namespace Curacaru.Backend.Application.CQRS.Deployment;

using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request for a deployment report.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="authId">The auth id used to check if user can view customer.</param>
/// <param name="year">The year of the report.</param>
/// <param name="month">The month of the report.</param>
/// <param name="customerId">The customer id the report is for.</param>
/// <param name="insuranceStatus">The insurance status to determine the corresponding report type.</param>
public class DeploymentReportRequest(
    Guid companyId,
    string authId,
    int year,
    int month,
    Guid customerId,
    InsuranceStatus insuranceStatus) : IRequest<byte[]>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid CustomerId { get; } = customerId;

    public InsuranceStatus InsuranceStatus { get; } = insuranceStatus;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class DeploymentReportRequestHandler(ICustomerRepository customerRepository, ICompanyRepository companyRepository, IReportService reportService)
    : IRequestHandler<DeploymentReportRequest, byte[]>
{
    public async Task<byte[]> Handle(DeploymentReportRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerId)
                       ?? throw new BadRequestException("Kunde existiert nicht.");

        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId)
                      ?? throw new BadRequestException("Firma existiert nicht.");

        var report = reportService.CreateDeploymentReport(company, customer, request.InsuranceStatus);
        return report;
    }
}