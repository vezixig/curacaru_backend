namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.AssignmentDeclaration;
using Core.Enums;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class AssignmentDeclarationsRequest(
    User user,
    int year,
    Guid? employeeId,
    Guid? customerId) : IRequest<List<GetAssignmentDeclarationListEntryDto>>
{
    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class AssignmentDeclarationsRequestHandler(
    ICustomerRepository customerRepository,
    IDocumentRepository documentRepository) : IRequestHandler<AssignmentDeclarationsRequest, List<GetAssignmentDeclarationListEntryDto>>
{
    public async Task<List<GetAssignmentDeclarationListEntryDto>> Handle(AssignmentDeclarationsRequest request, CancellationToken cancellationToken)
    {
        var customers = await customerRepository.GetCustomersForResponsibleEmployee(request.User.CompanyId, request.User.EmployeeId, request.CustomerId);
        customers = customers.Where(o => o.InsuranceStatus == InsuranceStatus.Statutory).ToList();

        var assignmentDeclarations =
            await documentRepository.GetAssignmentDeclarationsAsync(request.User.CompanyId, request.Year, request.CustomerId, request.EmployeeId);

        var result = customers.Select(
                o =>
                {
                    var assignment = assignmentDeclarations.Find(a => a.CustomerId == o.Id);
                    return new GetAssignmentDeclarationListEntryDto(
                        assignment?.Id,
                        o.Id,
                        o.FullNameReverse,
                        o.AssociatedEmployee?.FullName ?? "",
                        assignment is not null,
                        request.Year
                    );
                })
            .OrderBy(o => o.CustomerName)
            .ToList();

        return result;
    }
}