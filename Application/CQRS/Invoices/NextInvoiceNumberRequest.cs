namespace Curacaru.Backend.Application.CQRS.Invoices;

using AutoMapper;
using Core.DTO.Invoice;
using Infrastructure.Repositories;
using MediatR;

public class NextInvoiceNumberRequest(Guid companyId) : IRequest<GetInvoiceNumberDto>
{
    public Guid CompanyId { get; } = companyId;
}

internal class NextInvoiceNumberRequestHandler(IInvoiceRepository invoiceRepository, IMapper mapper) : IRequestHandler<NextInvoiceNumberRequest, GetInvoiceNumberDto>
{
    public async Task<GetInvoiceNumberDto> Handle(NextInvoiceNumberRequest request, CancellationToken cancellationToken)
        => new(await invoiceRepository.GetNextInvoiceNumberAsync(request.CompanyId));
}