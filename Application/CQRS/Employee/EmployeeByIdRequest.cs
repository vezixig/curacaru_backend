namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Entities;
using Infrastructure.repositories;
using MediatR;

/// <summary>Request for an employee by id.</summary>
/// <param name="companyId">The id of user's company.</param>
/// <param name="employeeId">The requested employee id.</param>
public class EmployeeByIdRequest(Guid companyId, string authId, string employeeId) : IRequest<Employee?>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid EmployeeId { get; } = Guid.Parse(employeeId);
}

internal class EmployeeByIdRequestHandler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeByIdRequest, Employee?>
{
    public async Task<Employee?> Handle(EmployeeByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != request.EmployeeId && !user.IsManager) throw new UnauthorizedAccessException("Du darfst nur dich selber ansehen");

        var employee = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.EmployeeId);
        return employee;
    }
}