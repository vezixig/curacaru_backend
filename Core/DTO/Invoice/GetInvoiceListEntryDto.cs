namespace Curacaru.Backend.Core.DTO.Invoice;

using Enums;

/// <summary>DTO representing a single entry in the invoice list.</summary>
public record GetInvoiceListEntryDto(
    int Year,
    int Month,
    string CustomerName,
    Guid CustomerId,
    ClearanceType ClearanceType,
    Guid? InvoiceId,
    string? InvoiceNumber)
{
}