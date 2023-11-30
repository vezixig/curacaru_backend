namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using MediatR;

public class EmployeeByIdRequest : IRequest<Employee>
{
    public EmployeeByIdRequest(string authId, string employeeId)
    {
        AuthId = authId;
        EmployeeId = Guid.Parse(employeeId);
    }

    public string AuthId { get; }

    public Guid EmployeeId { get; }
}

internal class EmployeeByIdRequestHandler : IRequestHandler<EmployeeByIdRequest, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeByIdRequestHandler(IEmployeeRepository employeeRepository)
        => _employeeRepository = employeeRepository;

    public async Task<Employee> Handle(EmployeeByIdRequest request, CancellationToken cancellationToken)
    {
        var companyId = (await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId))?.CompanyId
                        ?? throw new NotFoundException("Benutzer gehört zu keinem Unternehmen.");

        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId, companyId)
                       ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");
        return employee;
    }
}