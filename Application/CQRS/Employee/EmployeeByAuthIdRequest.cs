namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO.Employee;
using Infrastructure.repositories;
using MediatR;

/// <summary>Initializes a new instance of the <see cref="EmployeeByAuthIdRequest" /> class.</summary>
/// <param name="authId">The employee's auth id.</param>
public class EmployeeByAuthIdRequest(string authId) : IRequest<GetUserEmployeeDto?>
{
    public string AuthId { get; } = authId;
}

public class EmployeeByAuthIdRequestHandler(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
    : IRequestHandler<EmployeeByAuthIdRequest, GetUserEmployeeDto?>
{
    public async Task<GetUserEmployeeDto?> Handle(EmployeeByAuthIdRequest request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (employee == null) return null;

        var company = employee.CompanyId == null ? null : await companyRepository.GetCompanyById(employee.CompanyId.Value);

        return new GetUserEmployeeDto
        {
            CompanyId = employee.CompanyId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            IsManager = employee.IsManager,
            Id = employee.Id,
            CompanyName = company?.Name ?? ""
        };
    }
}