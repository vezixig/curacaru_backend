namespace Curacaru.Backend.Application.CQRS.Employee;

using Core.DTO.Employee;
using Infrastructure.Repositories;
using MediatR;

public class UpdateProfileRequest(string authId, UpdateProfileDto profile) : IRequest
{
    public string AuthId { get; } = authId;

    public UpdateProfileDto Profile { get; } = profile;
}

internal class UpdateProfileRequestHandler(IEmployeeRepository employeeRepository) : IRequestHandler<UpdateProfileRequest>
{
    public async Task Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        employee!.FirstName = request.Profile.FirstName;
        employee.LastName = request.Profile.LastName;
        employee.PhoneNumber = request.Profile.PhoneNumber;

        await employeeRepository.UpdateEmployeeAsync(employee);
    }
}