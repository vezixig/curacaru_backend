namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Services;
using MediatR;

public class DeleteEmployeeRequest(string authId, Guid employeeId) : IRequest
{
    public string AuthId { get; } = authId;

    public Guid EmployeeId { get; } = employeeId;
}

internal class DeleteEmployeeRequestHandler(IAuthService authService, IEmployeeRepository employeeRepository) : IRequestHandler<DeleteEmployeeRequest>
{
    public async Task Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.CompanyId == null) throw new NotFoundException("Benutzer gehört zu keinem Unternehmen.");

        if (user.Id == request.EmployeeId) throw new ForbiddenException("Du kannst dich nicht selbst löschen.");

        var employee = await employeeRepository.GetEmployeeByIdAsync(user.CompanyId.Value, request.EmployeeId)
                       ?? throw new NotFoundException("Mitarbeiter nicht gefunden.");

        await authService.DeleteUserAsync(employee.AuthId);
        await employeeRepository.DeleteEmployeeAsync(employee);
    }
}