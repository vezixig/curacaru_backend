namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Entities;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for an employee by id.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="employeeId">The requested employee id.</param>
public class EmployeeByIdRequest(User user, string employeeId) : IRequest<Employee?>
{
    /// <summary>Gets the employee id.</summary>
    public Guid EmployeeId { get; } = Guid.Parse(employeeId);

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class EmployeeByIdRequestHandler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeByIdRequest, Employee?>
{
    public async Task<Employee?> Handle(EmployeeByIdRequest request, CancellationToken cancellationToken)
    {
        if (request.User.EmployeeId != request.EmployeeId && !request.User.IsManager) throw new UnauthorizedAccessException("Du darfst nur dich selber ansehen");

        var employee = await employeeRepository.GetEmployeeByIdAsync(request.User.CompanyId, request.EmployeeId);
        return employee;
    }
}