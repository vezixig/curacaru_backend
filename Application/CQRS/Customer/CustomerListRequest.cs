namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for the customer list.</summary>
/// <param name="user">The authorized user.</param>
public class CustomerListRequest(User user) : IRequest<List<GetCustomerListEntryDto>>
{
    /// <summary>Gets the authorized user.</summary>
    public User User { get; } = user;
}

public class CustomerListRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<CustomerListRequest, List<GetCustomerListEntryDto>>
{
    public async Task<List<GetCustomerListEntryDto>> Handle(CustomerListRequest request, CancellationToken cancellationToken)
    {
        var customers = await customerRepository.GetCustomersAsync(request.User.CompanyId, request.User.IsManager ? null : request.User.EmployeeId);
        return mapper.Map<List<GetCustomerListEntryDto>>(customers);
    }
}