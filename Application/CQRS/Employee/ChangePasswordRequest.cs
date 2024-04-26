namespace Curacaru.Backend.Application.CQRS.Employee;

using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class ChangePasswordRequest(string AuthId) : IRequest
{
    public string AuthId { get; } = AuthId;
}

internal class ChangePasswordRequestHandler(IAuthService authService, IEmployeeRepository employeeRepository) : IRequestHandler<ChangePasswordRequest>
{
    public async Task Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        await authService.SendPasswordResetMail(user!.Email);
    }
}