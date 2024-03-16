namespace Curacaru.Backend.Infrastructure.Services;

using Core.Entities;
using Core.Enums;

/// <summary>Service to create reports.</summary>
public interface IReportService
{
    /// <summary>Creates a assignment declaration document.</summary>
    byte[] CreateAssignmentDeclaration(Company company, AssignmentDeclaration assignmentDeclaration);

    /// <summary>Creates a deployment report.</summary>
    byte[] CreateDeploymentReport(Company company, Customer customer, InsuranceStatus insuranceStatus);

    /// <summary>Generates a working time report.</summary>
    byte[] GenerateWorkingHoursReport(WorkingTimeReport report, List<Appointment> appointments);
}