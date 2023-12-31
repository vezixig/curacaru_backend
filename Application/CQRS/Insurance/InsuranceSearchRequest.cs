namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO.Insurance;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request to search for an insurance.</summary>
public class InsuranceSearchRequest(Guid companyId, string search) : IRequest<List<GetInsuranceDto>>
{
    public Guid CompanyId { get; } = companyId;

    public string Search { get; } = search;
}

internal class InsuranceSearchRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper)
    : IRequestHandler<InsuranceSearchRequest, List<GetInsuranceDto>>
{
    public async Task<List<GetInsuranceDto>> Handle(InsuranceSearchRequest request, CancellationToken cancellationToken)
    {
        var insurances = await insuranceRepository.SearchInsurancesByNameAsync(request.CompanyId, request.Search);
        return mapper.Map<List<GetInsuranceDto>>(insurances);
    }
}