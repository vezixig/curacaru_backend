namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to update an employee.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="employee">The modified employee.</param>
public class UpdateEmployeeRequest(User user, UpdateEmployeeDto employee) : IRequest<Employee>
{
    /// <summary>Gets the modified employee.</summary>
    public UpdateEmployeeDto Employee { get; } = employee;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class UpdateEmployeeRequestHandler(IEmployeeRepository employeeRepository) : IRequestHandler<UpdateEmployeeRequest, Employee>
{
    public async Task<Employee> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var currentEmployee = await employeeRepository.GetEmployeeByIdAsync(request.User.CompanyId, request.Employee.Id)
                              ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");

        if (currentEmployee.Id == request.User.EmployeeId) throw new NotFoundException("Du darfst deine eigene Rolle nicht ändern.");

        currentEmployee.Email = request.Employee.Email;
        currentEmployee.FirstName = request.Employee.FirstName;
        currentEmployee.LastName = request.Employee.LastName;
        currentEmployee.PhoneNumber = request.Employee.PhoneNumber;
        currentEmployee.IsManager = request.Employee.IsManager;

        var employee = await employeeRepository.UpdateEmployeeAsync(currentEmployee);
        return employee;
    }
}