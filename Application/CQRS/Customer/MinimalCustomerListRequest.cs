namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get deployments for an employee for a month.</summary>
public class MinimalCustomerListRequest(
    Guid companyId,
    string authId,
    InsuranceStatus? insuranceStatus) : IRequest<List<GetMinimalCustomerListEntryDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public InsuranceStatus? InsuranceStatus { get; } = insuranceStatus;
}

internal class DeploymentsRequestHandler(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<MinimalCustomerListRequest, List<GetMinimalCustomerListEntryDto>>
{
    public async Task<List<GetMinimalCustomerListEntryDto>> Handle(MinimalCustomerListRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId)
                   ?? throw new BadRequestException("Mitarbeiter existiert nicht.");

        var customers = await customerRepository.GetCustomersAsync(request.CompanyId, user!.IsManager ? null : user.Id, request.InsuranceStatus);

        return mapper.Map<List<GetMinimalCustomerListEntryDto>>(customers);
    }
}