namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get deployments for an employee for a month.</summary>
public class MinimalCustomerListForDeploymentReportsRequest(User user) : IRequest<List<GetMinimalCustomerListEntryDto>>
{
    public User User { get; } = user;
}

internal class MinimalCustomerListForDeploymentReportsRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<MinimalCustomerListForDeploymentReportsRequest, List<GetMinimalCustomerListEntryDto>>
{
    public async Task<List<GetMinimalCustomerListEntryDto>> Handle(MinimalCustomerListForDeploymentReportsRequest request, CancellationToken cancellationToken)
    {
        var customers = await customerRepository.GetCustomersForDeploymentReportsAsync(
            request.User.CompanyId,
            request.User.IsManager ? null : request.User.EmployeeId);

        return mapper.Map<List<GetMinimalCustomerListEntryDto>>(customers.OrderBy(o => o.FullNameReverse));
    }
}