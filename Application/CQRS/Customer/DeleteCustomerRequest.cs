namespace Curacaru.Backend.Application.CQRS.Customer;

using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class DeleteCustomerRequest(string authId, Guid customerId) : IRequest
{
    public string AuthId { get; } = authId;

    public Guid CustomerId { get; } = customerId;
}

internal class DeleteCustomerRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository) : IRequestHandler<DeleteCustomerRequest>
{
    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.CompanyId == null) throw new ForbiddenException("Benutzer gehört zu keinem Unternehmen.");

        var customer = await customerRepository.GetCustomerAsync(user.CompanyId.Value, request.CustomerId)
                       ?? throw new NotFoundException("Kunde nicht gefunden.");

        await customerRepository.DeleteCustomerAsync(customer);
    }
}