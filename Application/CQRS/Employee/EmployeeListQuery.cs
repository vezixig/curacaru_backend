namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using MediatR;

/// <summary>Query for getting all employees of the company the user is in.</summary>
public class EmployeeListQuery : IRequest<IEnumerable<Employee>>
{
    public EmployeeListQuery(string authId)
        => AuthId = authId;

    public string AuthId { get; }
}

internal class EmployeeListQueryHandler : IRequestHandler<EmployeeListQuery, IEnumerable<Employee>>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeListQueryHandler(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<Employee>> Handle(EmployeeListQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId)
                       ?? throw new BadRequestException("Employee not found");

        if (employee.CompanyId == null) throw new BadRequestException("Employee is not in a company");

        var employees = await _employeeRepository.GetEmployeesAsync(employee.CompanyId.Value);

        return employees;
    }
}