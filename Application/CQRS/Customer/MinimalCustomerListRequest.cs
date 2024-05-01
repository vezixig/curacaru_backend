namespace Curacaru.Backend.Application.CQRS.Customer;

using AutoMapper;
using Core.DTO.Customer;
using Core.Enums;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

/// <summary>A request to get deployments for an employee for a month.</summary>
public class MinimalCustomerListRequest(
    User user,
    InsuranceStatus? insuranceStatus,
    int? assignmentDeclarationYear) : IRequest<List<GetMinimalCustomerListEntryDto>>
{
    public int? AssignmentDeclarationYear { get; } = assignmentDeclarationYear;

    public InsuranceStatus? InsuranceStatus { get; } = insuranceStatus;

    public User User { get; } = user;
}

internal class DeploymentsRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<MinimalCustomerListRequest, List<GetMinimalCustomerListEntryDto>>
{
    public async Task<List<GetMinimalCustomerListEntryDto>> Handle(MinimalCustomerListRequest request, CancellationToken cancellationToken)
    {
        var customers = await customerRepository.GetCustomersAsync(
            request.User.CompanyId,
            request.User.IsManager ? null : request.User.EmployeeId,
            request.InsuranceStatus,
            request.AssignmentDeclarationYear);

        return mapper.Map<List<GetMinimalCustomerListEntryDto>>(customers.OrderBy(o => o.FullNameReverse));
    }
}