namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Exceptions;
using Infrastructure.repositories;
using MediatR;

public class DeleteEmployeeRequest : IRequest
{
    public DeleteEmployeeRequest(string authId, Guid employeeId)
    {
        AuthId = authId;
        EmployeeId = employeeId;
    }

    public string AuthId { get; }

    public Guid EmployeeId { get; }
}

internal class DeleteEmployeeRequestHandler : IRequestHandler<DeleteEmployeeRequest>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeRequestHandler(IEmployeeRepository employeeRepository)
        => _employeeRepository = employeeRepository;

    public async Task Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        var user = await _employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user.IsManager == false) throw new ForbiddenException("Nur Manager dürfen Mitarbeiter löschen.");
        if (user.CompanyId == null) throw new NotFoundException("Benutzer gehört zu keinem Unternehmen.");

        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId, user.CompanyId.Value)
                       ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");

        await _employeeRepository.DeleteEmployeeAsync(employee);
    }
}