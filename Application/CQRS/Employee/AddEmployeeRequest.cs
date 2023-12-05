namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Services;
using MediatR;

public class AddEmployeeRequest : IRequest<Employee>
{
    public AddEmployeeRequest(string authId, AddEmployeeDto employeeData)
    {
        AuthId = authId;
        EmployeeData = employeeData;
    }

    public string AuthId { get; }

    public AddEmployeeDto EmployeeData { get; }
}

internal class AddEmployeeRequestHandler : IRequestHandler<AddEmployeeRequest, Employee>
{
    private readonly IAuthService _authService;

    private readonly IEmailService _emailService;

    private readonly IEmployeeRepository _employeeRepository;

    public AddEmployeeRequestHandler(IAuthService authService, IEmailService emailService, IEmployeeRepository employeeRepository)
    {
        _authService = authService;
        _emailService = emailService;
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee> Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
    {
        var creator = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (!creator!.IsManager) throw new ForbiddenException("Nur Manager dürfen neue Benutzer anlegen.");

        if (await _employeeRepository.DoesEmailExistAsync(request.EmployeeData.Email))
            throw new BadRequestException("Die angegebene E-Mail Adresse wird bereits verwendet.");

        var user = await _authService.CreateUserAsync(request.EmployeeData.Email);

        _emailService.SendPasswordMail(request.EmployeeData.Email, user.Password);

        var employee = new Employee
        {
            AuthId = user.AuthId,
            Email = request.EmployeeData.Email,
            FirstName = request.EmployeeData.FirstName,
            IsManager = request.EmployeeData.IsManager,
            LastName = request.EmployeeData.LastName,
            PhoneNumber = request.EmployeeData.PhoneNumber,
            CompanyId = creator.CompanyId
        };

        employee = await _employeeRepository.AddEmployeeAsync(employee);
        return employee;
    }
}