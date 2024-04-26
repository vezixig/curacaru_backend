namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Entities;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Query for getting all employees of the company the user is in.</summary>
public class EmployeeListRequest(Guid companyId) : IRequest<IEnumerable<Employee>>
{
    public Guid CompanyId { get; } = companyId;
}

internal class EmployeeListQueryHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<EmployeeListRequest, IEnumerable<Employee>>
{
    public async Task<IEnumerable<Employee>> Handle(EmployeeListRequest request, CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetEmployeesAsync(request.CompanyId);
        return employees.OrderBy(o => o.FullName);
    }
}