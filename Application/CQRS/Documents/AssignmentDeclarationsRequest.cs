namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO;
using Core.DTO.AssignmentDeclaration;
using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class AssignmentDeclarationsRequest(
    User user,
    int year,
    Guid? employeeId,
    Guid? customerId,
    int page,
    int pageSize) : IRequest<PageDto<GetAssignmentDeclarationListEntryDto>>
{
    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class AssignmentDeclarationsRequestHandler(
    ICustomerRepository customerRepository,
    IDocumentRepository documentRepository) : IRequestHandler<AssignmentDeclarationsRequest, PageDto<GetAssignmentDeclarationListEntryDto>>
{
    public async Task<PageDto<GetAssignmentDeclarationListEntryDto>> Handle(AssignmentDeclarationsRequest request, CancellationToken cancellationToken)
    {
        if (!request.User.IsManager && request.EmployeeId.HasValue && request.EmployeeId != request.User.EmployeeId)
            throw new BadRequestException("Du darfst nur deine eigenen Kunden sehen.");

        var employeeId = request.User.IsManager ? request.EmployeeId : request.User.EmployeeId;

        var customers = await customerRepository.GetCustomersAsync(request.User.CompanyId, employeeId, customerId: request.CustomerId);
        customers = customers.Where(o => o.InsuranceStatus == InsuranceStatus.Statutory).ToList();

        var assignmentDeclarations =
            await documentRepository.GetAssignmentDeclarationsAsync(
                request.User.CompanyId,
                request.Year,
                request.CustomerId,
                employeeId);

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

        return new(
            result
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList(),
            request.Page,
            (int)Math.Ceiling((decimal)result.Count / request.PageSize));
    }
}