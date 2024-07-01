namespace Curacaru.Backend.Application.CQRS.Customer;

using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class DeleteCustomerRequest(User user, Guid customerId) : IRequest
{
    public Guid CustomerId => customerId;

    public User User => user;
}

internal class DeleteCustomerRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<DeleteCustomerRequest>
{
    public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.CustomerId)
                       ?? throw new NotFoundException("Kunde nicht gefunden.");

        if (customer.Status != CustomerStatus.Interested) throw new BadRequestException("Kunde ist kein Interessent.");

        await customerRepository.DeleteCustomerAsync(customer);
    }
}