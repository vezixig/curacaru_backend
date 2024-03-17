﻿namespace Curacaru.Backend.Infrastructure.Repositories;

using Core.Entities;

public interface IDocumentRepository
{
    /// <summary>Adds a new assignment declaration to the database.</summary>
    /// <param name="document">The assignment declaration to add.</param>
    Task AddAssignmentDeclarationAsync(AssignmentDeclaration document);

    /// <summary>Deletes an assignment declaration from the database.</summary>
    /// <param name="assignmentDeclaration">The assignment declaration to delete.</param>
    /// <returns>An awaitable task object.</returns>
    Task DeleteAssignmentDeclarationAsync(AssignmentDeclaration assignmentDeclaration);

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
}