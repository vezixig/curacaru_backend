namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO;
using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class CustomerRequest : IRequest<GetCustomerDto?>
{
    public CustomerRequest(Guid companyId, string employeeId)
    {
        CompanyId = companyId;
        EmployeeId = Guid.Parse(employeeId);
    }

    public Guid CompanyId { get; }

    public Guid EmployeeId { get; }
}

internal class CustomerRequestHandler : IRequestHandler<CustomerRequest, GetCustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IMapper _mapper;

    public CustomerRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerDto> Handle(CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerAsync(request.CompanyId, request.EmployeeId)
                       ?? throw new NotFoundException("Kunde nicht gefunden.");

        return _mapper.Map<GetCustomerDto>(customer);
    }
}