namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.AssignmentDeclaration;
using Core.Entities;
using Core.Enums;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class AssignmentDeclarationsRequest(
    Guid companyId,
    string authId,
    int year,
    Guid? employeeId,
    Guid? customerId) : IRequest<List<GetAssignmentDeclarationListEntryDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public int Year { get; } = year;
}

internal class AssignmentDeclarationsRequestHandler(
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IDocumentRepository documentRepository) : IRequestHandler<AssignmentDeclarationsRequest, List<GetAssignmentDeclarationListEntryDto>>
{
    public async Task<List<GetAssignmentDeclarationListEntryDto>> Handle(AssignmentDeclarationsRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var customers = await GetCustomersAsync(request, user);

        var assignmentDeclarations =
            await documentRepository.GetAssignmentDeclarationsAsync(request.CompanyId, request.Year, request.CustomerId, request.EmployeeId);

        var result = customers.Select(
                o =>
                {
                    var assignment = assignmentDeclarations.Find(a => a.CustomerId == o.Id);
                    return new GetAssignmentDeclarationListEntryDto(
                        assignment?.Id,
                        o.Id,
                        o.FullName,
                        o.AssociatedEmployee?.FullName ?? "",
                        assignment is not null,
                        request.Year
                    );
                })
            .OrderBy(o => o.CustomerName)
            .ToList();

        return result;
    }

    private async Task<List<Customer>> GetCustomersAsync(AssignmentDeclarationsRequest request, Employee? user)
    {
        var employeeId = request.EmployeeId ?? (user!.IsManager ? null : user.Id);
        List<Customer> customers = [];
        if (request.CustomerId.HasValue)
        {
            var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerId.Value, employeeId);
            if (customer is not null) customers.Add(customer);
        }
        else
        {
            var customerList = await customerRepository.GetCustomersAsync(request.CompanyId, employeeId);
            customers.AddRange(customerList);
        }

        return customers.Where(o => o.InsuranceStatus == InsuranceStatus.Statutory).ToList();
    }
}