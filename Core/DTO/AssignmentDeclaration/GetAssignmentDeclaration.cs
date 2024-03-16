namespace Curacaru.Backend.Core.DTO.AssignmentDeclaration;

/// <summary>Entry in a list of assignment declarations.</summary>
/// <param name="AssignmentDeclarationId">The id of the assignment declaration.</param>
/// <param name="CustomerId">The id of the customer.</param>
/// <param name="CustomerName">The name of the customer.</param>
/// <param name="EmployeeName">The name of the associated employee.</param>
/// <param name="IsSigned">Indicates whether the declaration is signed.</param>
/// <param name="Year">The year of the declaration.</param>
public record GetAssignmentDeclarationListEntryDto(
    Guid? AssignmentDeclarationId,
    Guid CustomerId,
    string CustomerName,
    string EmployeeName,
    bool IsSigned,
    int Year)
{
}