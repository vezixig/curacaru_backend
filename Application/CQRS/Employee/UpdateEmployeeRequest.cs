namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using MediatR;

public class UpdateEmployeeRequest : IRequest<Employee>
{
    public UpdateEmployeeRequest(string authId, UpdateEmployeeDto employee)
    {
        AuthId = authId;
        Employee = employee;
    }

    public string AuthId { get; }

    public UpdateEmployeeDto Employee { get; }
}

internal class UpdateEmployeeRequestHandler : IRequestHandler<UpdateEmployeeRequest, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeRequestHandler(IEmployeeRepository employeeRepository)
        => _employeeRepository = employeeRepository;

    public async Task<Employee> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var user = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        if (user!.CompanyId == null) throw new NotFoundException("Benutzer gehört zu keinem Unternehmen.");
        if (!user.IsManager) throw new ForbiddenException("Nur Manager dürfen Benutzer bearbeiten.");

        var currentEmploye = await _employeeRepository.GetEmployeeByIdAsync(request.Employee.Id, user.CompanyId.Value)
                             ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");

        currentEmploye.Email = request.Employee.Email;
        currentEmploye.FirstName = request.Employee.FirstName;
        currentEmploye.LastName = request.Employee.LastName;
        currentEmploye.PhoneNumber = request.Employee.PhoneNumber;
        currentEmploye.IsManager = request.Employee.IsManager;

        var employee = await _employeeRepository.UpdateEmployeeAsync(currentEmploye);
        return employee;
    }
}