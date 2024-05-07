namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Entities;
using Core.Enums;
using Infrastructure.Repositories;
using MediatR;

public class AddCustomerRequest(Guid companyId, AddCustomerDto customer) : IRequest<GetCustomerListEntryDto>
{
    public Guid CompanyId { get; } = companyId;

    public AddCustomerDto Customer { get; } = customer;
}

public class AddCustomerRequestHandler(ICustomerRepository customerRepository, IMapper mapper) : IRequestHandler<AddCustomerRequest, GetCustomerListEntryDto>
{
    public async Task<GetCustomerListEntryDto> Handle(AddCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request.Customer);
        customer.CompanyId = request.CompanyId;
        customer.AssociatedEmployee = request.Customer.AssociatedEmployeeId.HasValue ? new Employee { Id = request.Customer.AssociatedEmployeeId.Value } : null;
        customer.ZipCity = request.Customer.ZipCode != null ? new ZipCity { ZipCode = request.Customer.ZipCode } : null;
        customer.Insurance = request.Customer.InsuranceId.HasValue ? new Insurance { Id = request.Customer.InsuranceId.Value } : null;
        customer.Status ??= CustomerStatus.Customer;

        customer = await customerRepository.AddCustomerAsync(customer);
        return mapper.Map<GetCustomerListEntryDto>(customer);
    }
}