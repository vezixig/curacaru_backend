namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Infrastructure.repositories;
using MediatR;

public class EmployeeByAuthIdQuery : IRequest<GetUserEmployeeDto?>
{
    /// <summary>Initializes a new instance of the <see cref="EmployeeByAuthIdQuery" /> class.</summary>
    /// <param name="authId">The employee's auth id.</param>
    public EmployeeByAuthIdQuery(string authId)
        => AuthId = authId;

    public string AuthId { get; }
}

public class EmployeeByAuthIdQueryHandler : IRequestHandler<EmployeeByAuthIdQuery, GetUserEmployeeDto?>
{
    private readonly ICompanyRepository _companyRepository;

    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeByAuthIdQueryHandler(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<GetUserEmployeeDto?> Handle(EmployeeByAuthIdQuery request, CancellationToken cancellationToken)
    {
        var employe = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (employe == null) return null;

        var company = employe.CompanyId == null ? null : await _companyRepository.GetCompanyById(employe.CompanyId.Value);

        return new GetUserEmployeeDto
        {
            CompanyId = employe.CompanyId,
            FirstName = employe.FirstName,
            LastName = employe.LastName,
            CompanyName = company?.Name ?? ""
        };
    }
}