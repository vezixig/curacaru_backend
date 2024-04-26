namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for a customer.</summary>
/// <param name="user">The authorized user.</param>
/// <param name="customerId">The customer id.</param>
public class CustomerRequest(User user, Guid customerId) : IRequest<GetCustomerDto?>
{
    /// <summary>Gets the customer id.</summary>
    public Guid CustomerId { get; } = customerId;

    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

internal class CustomerRequestHandler(ICustomerRepository customerRepository, IMapper mapper) : IRequestHandler<CustomerRequest, GetCustomerDto?>
{
    public async Task<GetCustomerDto?> Handle(CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(
            request.User.CompanyId,
            request.CustomerId,
            request.User.IsManager ? null : request.User.EmployeeId);
        return mapper.Map<GetCustomerDto>(customer);
    }
}