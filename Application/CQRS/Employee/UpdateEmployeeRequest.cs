namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using MediatR;

public class UpdateEmployeeRequest(Guid companyId, UpdateEmployeeDto employee) : IRequest<Employee>
{
    public Guid CompanyId { get; } = companyId;

    public UpdateEmployeeDto Employee { get; } = employee;
}

internal class UpdateEmployeeRequestHandler(IEmployeeRepository employeeRepository) : IRequestHandler<UpdateEmployeeRequest, Employee>
{
    public async Task<Employee> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var currentEmployee = await employeeRepository.GetEmployeeByIdAsync(request.Employee.Id, request.CompanyId)
                              ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");

        currentEmployee.Email = request.Employee.Email;
        currentEmployee.FirstName = request.Employee.FirstName;
        currentEmployee.LastName = request.Employee.LastName;
        currentEmployee.PhoneNumber = request.Employee.PhoneNumber;
        currentEmployee.IsManager = request.Employee.IsManager;

        var employee = await employeeRepository.UpdateEmployeeAsync(currentEmployee);
        return employee;
    }
}