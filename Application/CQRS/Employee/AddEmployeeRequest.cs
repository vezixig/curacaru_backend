namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class AddEmployeeRequest(Guid companyId, AddEmployeeDto employeeData) : IRequest<Employee>
{
    public Guid CompanyId { get; } = companyId;

    public AddEmployeeDto EmployeeData { get; } = employeeData;
}

internal class AddEmployeeRequestHandler(IAuthService authService, IEmailService emailService, IEmployeeRepository employeeRepository)
    : IRequestHandler<AddEmployeeRequest, Employee>
{
    public async Task<Employee> Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
    {
        if (await employeeRepository.DoesEmailExistAsync(request.EmployeeData.Email))
            throw new BadRequestException("Die angegebene E-Mail Adresse wird bereits verwendet.");

        var user = await authService.CreateUserAsync(request.EmployeeData.Email);

        emailService.SendPasswordMail(request.EmployeeData.Email, user.Password);

        var employee = new Employee
        {
            AuthId = user.AuthId,
            Email = request.EmployeeData.Email,
            FirstName = request.EmployeeData.FirstName,
            IsManager = request.EmployeeData.IsManager,
            LastName = request.EmployeeData.LastName,
            PhoneNumber = request.EmployeeData.PhoneNumber,
            CompanyId = request.CompanyId
        };

        employee = await employeeRepository.AddEmployeeAsync(employee);
        return employee;
    }
}