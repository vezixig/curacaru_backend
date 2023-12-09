namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO;
using Infrastructure.Repositories;
using MediatR;

public class CustomerListRequest : IRequest<List<GetCustomerListEntryDto>>
{
    public CustomerListRequest(Guid companyId)
        => CompanyId = companyId;

    public Guid CompanyId { get; }
}

public class CustomerListRequestHandler : IRequestHandler<CustomerListRequest, List<GetCustomerListEntryDto>>
{
    private readonly IMapper _mapper;

    public CustomerListRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _mapper = mapper;
        CustomerRepository = customerRepository;
    }

    public ICustomerRepository CustomerRepository { get; }

    public async Task<List<GetCustomerListEntryDto>> Handle(CustomerListRequest request, CancellationToken cancellationToken)
    {
        var customers = await CustomerRepository.GetCustomersAsync(request.CompanyId);
        return _mapper.Map<List<GetCustomerListEntryDto>>(customers);
    }
}