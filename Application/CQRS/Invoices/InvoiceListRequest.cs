namespace Curacaru.Backend.Application.CQRS.Invoices;

using Core.DTO.Invoice;
using Infrastructure.Repositories;
using MediatR;

public class InvoiceListRequest(
    Guid companyId,
    int year,
    int month,
    Guid? customerId) : IRequest<List<GetInvoiceListEntryDto>>
{
    public Guid CompanyId { get; } = companyId;

    public Guid? CustomerId { get; } = customerId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class InvoiceListRequestHandler(IDocumentRepository documentRepository) : IRequestHandler<InvoiceListRequest, List<GetInvoiceListEntryDto>>
{
    public async Task<List<GetInvoiceListEntryDto>> Handle(InvoiceListRequest request, CancellationToken cancellationToken)
    {
        var deploymentReports = await documentRepository.GetDeploymentReportsAsync(request.CompanyId, request.CustomerId, request.Year, request.Month);

        return deploymentReports.Select(
                i =>
                    new GetInvoiceListEntryDto(i.Year, i.Month, i.Customer.FullName, i.CustomerId, i.ClearanceType, i.Invoice?.Id, i.Invoice?.InvoiceNumber))
            .ToList();
    }
}