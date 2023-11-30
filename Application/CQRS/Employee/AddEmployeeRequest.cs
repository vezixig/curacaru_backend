namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
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
    private readonly IEmployeeRepository _employeeRepository;

    public AddEmployeeRequestHandler(IEmployeeRepository employeeRepository)
        => _employeeRepository = employeeRepository;

    public async Task<Employee> Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
    {
        var creator = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (!creator!.IsManager) throw new ForbiddenException("Nur Manager dürfen neue Benutzer anlegen.");

        // todo: check if email exists

        // todo: register with Auth0

        var employee = new Employee
        {
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