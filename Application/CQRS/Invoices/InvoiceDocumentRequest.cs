namespace Curacaru.Backend.Application.CQRS.Invoices;

using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class InvoiceDocumentRequest(Guid companyId, Guid invoiceId) : IRequest<byte[]>
{
    public Guid CompanyId { get; } = companyId;

    public Guid InvoiceId { get; } = invoiceId;
}

public class InvoiceDocumentRequestHandler(ICompanyRepository companyRepository, IInvoiceRepository invoiceRepository, IReportService reportService)
    : IRequestHandler<InvoiceDocumentRequest, byte[]>
{
    public async Task<byte[]> Handle(InvoiceDocumentRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);

        var invoice = await invoiceRepository.GetInvoiceAsync(request.CompanyId, request.InvoiceId)
                      ?? throw new InvalidOperationException("Rechnung nicht gefunden.");

        return reportService.CreateInvoiceDocument(company!, invoice);
    }
}