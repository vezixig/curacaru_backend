namespace Curacaru.Backend.Application.CQRS.Invoices;

using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class DeleteInvoiceRequest(Guid companyId, Guid invoiceId) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public Guid InvoiceId { get; } = invoiceId;
}

internal class DeleteInvoiceRequestHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<DeleteInvoiceRequest>
{
    public async Task Handle(DeleteInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = await invoiceRepository.GetInvoiceAsync(request.CompanyId, request.InvoiceId)
                      ?? throw new NotFoundException("Rechnung wurde nicht gefunden.");

        await invoiceRepository.DeleteInvoiceAsync(invoice);
    }
}