namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to delete an assignment declaration.</summary>
/// <param name="companyId">The id of the company the customer of the assignment declaration belongs to.</param>
/// <param name="assignmentDeclarationId">The id of the assignment declaration to delete.</param>
public class DeleteAssignmentDeclarationRequest(Guid companyId, Guid assignmentDeclarationId) : IRequest
{
    /// <summary>Gets the id of the assignment declaration to delete.</summary>
    public Guid AssignmentDeclarationId { get; } = assignmentDeclarationId;

    /// <summary>Gets the company id the customer of the assignment declaration belongs to.</summary>
    public Guid CompanyId { get; } = companyId;
}

internal class DeleteAssignmentDeclarationRequestHandler(IDocumentRepository documentRepository)
    : IRequestHandler<DeleteAssignmentDeclarationRequest>
{
    public async Task Handle(DeleteAssignmentDeclarationRequest request, CancellationToken cancellationToken)
    {
        var assignmentDeclaration = await documentRepository.GetAssignmentDeclarationByIdAsync(request.CompanyId, request.AssignmentDeclarationId)
                                    ?? throw new NotFoundException("Die Abtretungserklärung konnte nicht gefunden werden.");
        await documentRepository.DeleteAssignmentDeclarationAsync(assignmentDeclaration);
    }
}