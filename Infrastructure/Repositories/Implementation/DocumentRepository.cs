namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

internal class DocumentRepository(DataContext dataContext) : IDocumentRepository
{
    public Task AddAssignmentDeclarationAsync(AssignmentDeclaration document)
    {
        dataContext.Attach(document.Customer);
        dataContext.AssignmentDeclarations.Add(document);
        return dataContext.SaveChangesAsync();
    }

    public Task<bool> DoesAssignmentDeclarationExistAsync(Guid customerId, int year)
        => dataContext.AssignmentDeclarations.AnyAsync(o => o.CustomerId == customerId && o.Year == year);

    public Task<AssignmentDeclaration?> GetAssignmentDeclarationAsync(int requestYear, Guid requestCustomerId)
        => dataContext.AssignmentDeclarations
            .Include(o => o.CustomerZipCity)
            .Include(o => o.InsuranceZipCity)
            .FirstOrDefaultAsync(o => o.Year == requestYear && o.CustomerId == requestCustomerId);

    public Task<List<AssignmentDeclaration>> GetAssignmentDeclarationsAsync(
        Guid companyId,
        int year,
        Guid? customerId,
        Guid? employeeId)
    {
        var query = dataContext.AssignmentDeclarations
            .Where(o => o.CompanyId == companyId && o.Year == year);

        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId);

        if (employeeId.HasValue) query = query.Where(o => o.Customer.AssociatedEmployeeId == employeeId);

        return query.ToListAsync();
    }
}