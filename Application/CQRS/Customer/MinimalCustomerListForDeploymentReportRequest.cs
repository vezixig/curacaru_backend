namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get deployments for an employee for a month.</summary>
public class MinimalCustomerListForDeploymentReportsRequest(
    Guid companyId,
    string authId) : IRequest<List<GetMinimalCustomerListEntryDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;
}

internal class MinimalCustomerListForDeploymentReportsRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<MinimalCustomerListForDeploymentReportsRequest, List<GetMinimalCustomerListEntryDto>>
{
    public async Task<List<GetMinimalCustomerListEntryDto>> Handle(MinimalCustomerListForDeploymentReportsRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId)
                   ?? throw new BadRequestException("Mitarbeiter existiert nicht.");

        var customers = await customerRepository.GetCustomersForDeploymentReportsAsync(
            request.CompanyId,
            user!.IsManager ? null : user.Id);

        return mapper.Map<List<GetMinimalCustomerListEntryDto>>(customers);
    }
}