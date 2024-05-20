namespace Curacaru.Backend.Application.CQRS.Invoices;

using Core.DTO;
using Core.DTO.Invoice;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for a list of invoices for a specific month and year.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="year">The year to get the invoices for.</param>
/// <param name="month">The mont of the year to get the invoices for</param>
/// <param name="customerId">An optional customer id to filter by.</param>
/// <param name="page">The page to return.</param>
/// <param name="pageSize">The number of results per page.</param>
public class InvoiceListRequest(
    Guid companyId,
    int year,
    int month,
    Guid? customerId,
    int page,
    int pageSize) : IRequest<PageDto<GetInvoiceListEntryDto>>
{
    public Guid CompanyId { get; } = companyId;

    public Guid? CustomerId { get; } = customerId;

    public int Month { get; } = month;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    public int Year { get; } = year;
}

internal class InvoiceListRequestHandler(IDocumentRepository documentRepository) : IRequestHandler<InvoiceListRequest, PageDto<GetInvoiceListEntryDto>>
{
    public async Task<PageDto<GetInvoiceListEntryDto>> Handle(InvoiceListRequest request, CancellationToken cancellationToken)
    {
        var deploymentReportCount = await documentRepository.GetDeploymentReportCountAsync(request.CompanyId, request.CustomerId, request.Year, request.Month);
        var deploymentReports = await documentRepository.GetDeploymentReportsAsync(
            request.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month,
            page: request.Page,
            pageSize: request.PageSize);

        return new(
            deploymentReports.Select(
                    report =>
                        new GetInvoiceListEntryDto(
                            ClearanceType: report.ClearanceType,
                            CustomerId: report.Customer.Id,
                            CustomerName: report.Customer.FullNameReverse,
                            EmployeeName: report.Invoice?.SignedEmployee.FullName ?? "",
                            InvoiceId: report.Invoice?.Id,
                            InvoiceNumber: report.Invoice?.InvoiceNumber,
                            Month: report.Month,
                            Year: report.Year))
                .ToList(),
            request.Page,
            (int)Math.Ceiling((double)deploymentReportCount / request.PageSize));
    }
}