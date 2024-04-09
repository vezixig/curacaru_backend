namespace Curacaru.Backend.Application.CQRS.Invoices;

using Core.DTO.Invoice;
using Infrastructure.Repositories;
using MediatR;

public class NextInvoiceNumberRequest(Guid companyId) : IRequest<GetInvoiceNumberDto>
{
    public Guid CompanyId { get; } = companyId;
}

internal class NextInvoiceNumberRequestHandler(IInvoiceRepository invoiceRepository) : IRequestHandler<NextInvoiceNumberRequest, GetInvoiceNumberDto>
{
    public async Task<GetInvoiceNumberDto> Handle(NextInvoiceNumberRequest request, CancellationToken cancellationToken)
        => new(await invoiceRepository.GetNextInvoiceNumberAsync(request.CompanyId));
}