namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class CustomerListRequest(Guid companyId, string authId) : IRequest<List<GetCustomerListEntryDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

public class CustomerListRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<CustomerListRequest, List<GetCustomerListEntryDto>>
{
    public async Task<List<GetCustomerListEntryDto>> Handle(CustomerListRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var customers = await customerRepository.GetCustomersForResponsibleEmployee(request.CompanyId, user!.IsManager ? null : user.Id);
        return mapper.Map<List<GetCustomerListEntryDto>>(customers);
    }
}