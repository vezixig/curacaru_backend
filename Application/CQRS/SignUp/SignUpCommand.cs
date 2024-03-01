namespace Curacaru.Backend.Application.CQRS.SignUp;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Services;
using MediatR;

public class SignUpCommand(SignUpDto signUpDto, string authId) : IRequest<GetEmployeeDto>
{
    public string AuthId { get; } = authId;

    public SignUpDto SignUpDto { get; } = signUpDto;
}

internal class SignUpCommandHandler(
    IAuthService authService,
    ICompanyRepository companyRepository,
    IDatabaseService databaseService,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<SignUpCommand, GetEmployeeDto>
{
    public async Task<GetEmployeeDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId) != null) throw new BadRequestException("Employee already exists");

        var transaction = await databaseService.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get email from Auth0
            var email = await authService.GetMailAsync(request.AuthId);

            // Add the company
            var company = new Company
            {
                Name = request.SignUpDto.CompanyName
            };

            company = await companyRepository.AddCompanyAsync(company);

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

            employee = await employeeRepository.AddEmployeeAsync(employee);

            await transaction.CommitAsync(cancellationToken);

            return new()
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