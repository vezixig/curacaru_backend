namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Infrastructure.Repositories;
using MediatR;

public class CustomerRequest(Guid companyId, Guid employeeId) : IRequest<GetCustomerDto?>
{
    public Guid CompanyId { get; } = companyId;

    public Guid EmployeeId { get; } = employeeId;
}

internal class CustomerRequestHandler(ICustomerRepository customerRepository, IMapper mapper) : IRequestHandler<CustomerRequest, GetCustomerDto?>
{
    public async Task<GetCustomerDto?> Handle(CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.EmployeeId);
        return mapper.Map<GetCustomerDto>(customer);
    }
}