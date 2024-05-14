namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;
using Core.Enums;

public interface IDocumentRepository
{
    /// <summary>Adds a new assignment declaration to the database.</summary>
    /// <param name="document">The assignment declaration to add.</param>
    Task AddAssignmentDeclarationAsync(AssignmentDeclaration document);

    /// <summary>Adds a new deployment report.</summary>
    /// <param name="deploymentReport">The report to add.</param>
    /// <returns>An awaitable task object.</returns>
    Task AddDeploymentReportAsync(DeploymentReport deploymentReport);

    /// <summary>Deletes an assignment declaration from the database.</summary>
    /// <param name="assignmentDeclaration">The assignment declaration to delete.</param>
    /// <returns>An awaitable task object.</returns>
    Task DeleteAssignmentDeclarationAsync(AssignmentDeclaration assignmentDeclaration);

    /// <summary>Deletes a deployment report.</summary>
    /// <param name="deploymentReport">The report to delete</param>
    /// <returns>An awaitable task object.</returns>
    Task DeleteDeploymentReportAsync(DeploymentReport deploymentReport);

    /// <summary>Checks if an assignment declaration for the given year already exists.</summary>
    /// <param name="customerId">The customer id.</param>
    /// <param name="year">The year of the assignment declaration.</param>
    /// <returns>True if the assignment declaration exists, otherwise false.</returns>
    Task<bool> DoesAssignmentDeclarationExistAsync(Guid customerId, int year);

    /// <summary>Gets the assignment declaration for the given year and customer.</summary>
    /// <param name="requestYear">The year of the assignment declaration.</param>
    /// <param name="requestCustomerId">The id of the customer.</param>
    /// <returns>The assignment declaration or null if none is found.</returns>
    Task<AssignmentDeclaration?> GetAssignmentDeclarationAsync(int requestYear, Guid requestCustomerId);

    /// <summary>Gets the assignment declaration by its id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="assignmentDeclarationId">The id of the assignment declaration.</param>
    /// <returns>The assignment declaration or null if none is found.</returns>
    Task<AssignmentDeclaration?> GetAssignmentDeclarationByIdAsync(Guid companyId, Guid assignmentDeclarationId);

    /// <summary>Gets the assignment declarations for the given year and filters optional by employee.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="year">The year of the assignment declarations.</param>
    /// <param name="customerId">Optional the id of the customer to filter by.</param>
    /// <param name="employeeId">Optional the id of the employee to filter by.</param>
    /// <returns>A list of assignment declarations.</returns>
    Task<List<AssignmentDeclaration>> GetAssignmentDeclarationsAsync(
        Guid companyId,
        int year,
        Guid? customerId,
        Guid? employeeId);

    /// <summary>Gets the count of assignment declarations for the provided filters.</summary>
    /// ///
    /// <param name="companyId">The company id.</param>
    /// <param name="year">The year of the assignment declarations.</param>
    /// <param name="customerId">Optional the id of the customer to filter by.</param>
    /// <param name="employeeId">Optional the id of the employee to filter by.</param>
    Task<int> GetAssignmentDeclarationsCountAsync(
        Guid companyId,
        int year,
        Guid? customerId,
        Guid? employeeId);

    /// <summary>Gets a deployment report by its id.</summary>
    /// <param name="companyId">The company id.</param>
    /// <param name="reportId">The id of the deployment report.</param>
    /// <returns>The deployment report or null if none is found.</returns>
    Task<DeploymentReport?> GetDeploymentReportByIdAsync(Guid companyId, Guid reportId);

    /// <summary>Gets the number of deployment reports for the given filters.</summary>
    Task<int> GetDeploymentReportCountAsync(
        Guid companyId,
        Guid? customerId,
        int year,
        int month);

    /// <summary>Checks if a deployment report for the given year and month already exists.</summary>
    Task<Guid?> GetDeploymentReportId(
        Guid companyId,
        Guid customerId,
        int year,
        int month,
        ClearanceType clearanceType);

    /// <summary>Gets the deployment reports for the given filters.</summary>
    Task<List<DeploymentReport>> GetDeploymentReportsAsync(
        Guid companyId,
        Guid? customerId,
        int year,
        int month,
        ClearanceType? clearanceType = null,
        bool includeAppointments = false,
        int? page = null,
        int? pageSize = null);
}