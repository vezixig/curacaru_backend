namespace Curacaru.Backend.Core.DTO.Invoice;

public record AddInvoiceDto(
    Guid DeploymentReportId,
    DateOnly InvoiceDate,
    string InvoiceNumber,
    string Signature)
{
}