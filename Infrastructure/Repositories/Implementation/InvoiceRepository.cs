namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using System.Text.RegularExpressions;
using Core.Attributes;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class InvoiceRepository(DataContext dataContext) : IInvoiceRepository
{
    public Task AddInvoiceAsync(Invoice invoice)
    {
        dataContext.Attach(invoice.SignedEmployee);
        dataContext.Attach(invoice.DeploymentReport);
        dataContext.Invoices.Add(invoice);
        return dataContext.SaveChangesAsync();
    }

    public Task DeleteInvoiceAsync(Invoice invoice)
    {
        dataContext.Invoices.Remove(invoice);
        return dataContext.SaveChangesAsync();
    }

    public async Task<string> GetNextInvoiceNumberAsync(Guid companyId)
    {
        var invoiceNumbers = await dataContext.Invoices
            .Where(
                o => o.CompanyId == companyId && o.DeploymentReport.Year == DateTime.Today.Year)
            .Select(o => o.InvoiceNumber)
            .ToListAsync();

        invoiceNumbers = invoiceNumbers.Where(o => Regex.IsMatch(o, @"^2024-\d{4}$")).Order().ToList();

        return invoiceNumbers.Any()
            ? DateTime.Today.Year + "-" + (int.Parse(invoiceNumbers.Last()[5..]) + 1).ToString("D4") // don't use [-1] instead of last(), will throw exception
            : DateTime.Today.Year + "-0001";
    }

    public Task<Invoice?> GetInvoiceAsync(Guid companyId, Guid invoiceId)
        => dataContext.Invoices
            .AsTracking()
            .Include(o => o.DeploymentReport)
            .ThenInclude(o => o.Customer)
            .ThenInclude(o => o.ZipCity)
            .Include(o => o.DeploymentReport)
            .ThenInclude(o => o.Insurance)
            .ThenInclude(o => o.ZipCity)
            .Include(o => o.SignedEmployee)
            .FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == invoiceId);
}