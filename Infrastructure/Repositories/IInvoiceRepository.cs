namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IInvoiceRepository
{
    /// <summary>Updates an invoice.</summary>
    /// <param name="invoice">The invoice to add.</param>
    Task AddInvoiceAsync(Invoice invoice);

    /// <summary>Deletes an invoice.</summary>
    /// <param name="invoice">The invoice to delete</param>
    /// <returns>An awaitable task object.</returns>
    Task DeleteInvoiceAsync(Invoice invoice);

    /// <summary>Gets an invoice.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="invoiceId">The invoice id.</param>
    /// <returns>An invoice or null if none is found.</returns>
    Task<Invoice?> GetInvoiceAsync(Guid companyId, Guid invoiceId);

    /// <summary>Gets the next invoice number for a company.</summary>
    /// <param name="companyId">The company id.</param>
    Task<string> GetNextInvoiceNumberAsync(Guid companyId);
}