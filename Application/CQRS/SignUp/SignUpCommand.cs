namespace Curacaru.Backend.Application.CQRS.SignUp;

using Core.DTO;
using Core.Entities;
using Infrastructure.repositories;
using Infrastructure.Services;
using MediatR;

public class SignUpCommand : IRequest<GetEmployeeDto>
{
    public SignUpCommand(SignUpDto signUpDto, string authId)
    {
        SignUpDto = signUpDto;
        AuthId = authId;
    }

    public string AuthId { get; }

    public SignUpDto SignUpDto { get; }
}

internal class SignUpCommandHandler : IRequestHandler<SignUpCommand, GetEmployeeDto>
{
    private readonly IAuthService _authService;

    private readonly ICompanyRepository _companyRepository;

    private readonly IDatabaseService _databaseService;

    private readonly IEmployeeRepository _employeeRepository;

    public SignUpCommandHandler(
        IAuthService authService,
        ICompanyRepository companyRepository,
        IDatabaseService databaseService,
        IEmployeeRepository employeeRepository)
    {
        _authService = authService;
        _companyRepository = companyRepository;
        _databaseService = databaseService;
        _employeeRepository = employeeRepository;
    }

    public async Task<GetEmployeeDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId) != null) throw new Exception("Employee already exists");

        var transaction = await _databaseService.GetTransactionAsync(cancellationToken);

        try
        {
            // Get email from Auth0
            var email = await _authService.GetMailAsync(request.AuthId);

            // Add the company
            var company = new Company
            {
                Name = request.SignUpDto.CompanyName
            };

            company = await _companyRepository.AddCompanyAsync(company);

            // Add the employee
            var employee = new Employee
            {
                AuthId = request.AuthId,
                CompanyId = company.Id,
                Email = email,
                FirstName = request.SignUpDto.FirstName,
                IsManager = true,
                LastName = request.SignUpDto.LastName
            };

            employee = await _employeeRepository.AddEmployeeAsync(employee);

            await transaction.CommitAsync(cancellationToken);

            return new GetEmployeeDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName
            };
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}