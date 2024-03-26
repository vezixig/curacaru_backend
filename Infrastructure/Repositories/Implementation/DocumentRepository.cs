namespace Curacaru.Backend.Infrastructure.Repositories.Implementation;

using Core.Attributes;
using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;

[Repository]
internal class DocumentRepository(DataContext dataContext) : IDocumentRepository
{
    public Task AddAssignmentDeclarationAsync(AssignmentDeclaration document)
    {
        dataContext.Attach(document.Customer);
        dataContext.AssignmentDeclarations.Add(document);
        return dataContext.SaveChangesAsync();
    }

    public Task AddDeploymentReportAsync(DeploymentReport deploymentReport)
    {
        dataContext.Attach(deploymentReport.Customer);
        dataContext.AttachRange(deploymentReport.Appointments);
        dataContext.DeploymentReports.Add(deploymentReport);
        return dataContext.SaveChangesAsync();
    }

    public Task DeleteAssignmentDeclarationAsync(AssignmentDeclaration assignmentDeclaration)
    {
        dataContext.AssignmentDeclarations.Remove(assignmentDeclaration);
        return dataContext.SaveChangesAsync();
    }

    public Task DeleteDeploymentReportAsync(DeploymentReport deploymentReport)
    {
        dataContext.DeploymentReports.Remove(deploymentReport);
        return dataContext.SaveChangesAsync();
    }

    public Task<bool> DoesAssignmentDeclarationExistAsync(Guid customerId, int year)
        => dataContext.AssignmentDeclarations.AnyAsync(o => o.CustomerId == customerId && o.Year == year);

    public Task<bool> DoesDeploymentReportExistAsync(
        Guid companyId,
        Guid customerId,
        int year,
        int month,
        ClearanceType clearanceType)
        => dataContext.DeploymentReports.AnyAsync(
            o => o.CompanyId == companyId && o.CustomerId == customerId && o.Year == year && o.Month == month && o.ClearanceType == clearanceType);

    public Task<AssignmentDeclaration?> GetAssignmentDeclarationAsync(int requestYear, Guid requestCustomerId)
        => dataContext.AssignmentDeclarations
            .Include(o => o.CustomerZipCity)
            .Include(o => o.InsuranceZipCity)
            .FirstOrDefaultAsync(o => o.Year == requestYear && o.CustomerId == requestCustomerId);

    public Task<AssignmentDeclaration?> GetAssignmentDeclarationByIdAsync(Guid companyId, Guid assignmentDeclarationId)
        => dataContext.AssignmentDeclarations.FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == assignmentDeclarationId);

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

    public Task<DeploymentReport?> GetDeploymentReportByIdAsync(Guid companyId, Guid reportId)
        => dataContext.DeploymentReports.AsTracking().Include(o => o.Appointments).FirstOrDefaultAsync(o => o.CompanyId == companyId && o.Id == reportId);

    public Task<List<DeploymentReport>> GetDeploymentReportsAsync(
        Guid companyId,
        Guid? customerId,
        int year,
        int month,
        ClearanceType? clearanceType = null,
        bool includeAppointments = false)
    {
        var query = dataContext.DeploymentReports.Where(o => o.CompanyId == companyId && o.Year == year && o.Month == month);

        if (customerId.HasValue) query = query.Where(o => o.CustomerId == customerId);
        if (clearanceType.HasValue) query = query.Where(o => o.ClearanceType == clearanceType.Value);

        if (includeAppointments)
            query = query.Include(o => o.Appointments)
                .Include(o => o.Customer)
                .ThenInclude(o => o.ZipCity)
                .Include(o => o.Customer.Insurance)
                .ThenInclude(o => o.ZipCity)
                .Include(o => o.Appointments)
                .ThenInclude(o => o.Employee)
                .Include(o => o.Appointments)
                .ThenInclude(o => o.EmployeeReplacement);
        ;
        return query.ToListAsync();
    }
}