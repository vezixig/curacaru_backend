namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO;
using Core.Entities;
using Infrastructure.Repositories;
using MediatR;

public class AddCustomerRequest : IRequest<GetCustomerListEntryDto>
{
    public AddCustomerRequest(Guid companyId, AddCustomerDto customer)
    {
        CompanyId = companyId;
        Customer = customer;
    }

    public Guid CompanyId { get; }

    public AddCustomerDto Customer { get; }
}

public class AddCustomerRequestHandler : IRequestHandler<AddCustomerRequest, GetCustomerListEntryDto>
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IMapper _mapper;

    public AddCustomerRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerListEntryDto> Handle(AddCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = _mapper.Map<Customer>(request.Customer);
        customer.CompanyId = request.CompanyId;

        customer = await _customerRepository.AddCustomerAsync(customer);
        return _mapper.Map<GetCustomerListEntryDto>(customer);
    }
}