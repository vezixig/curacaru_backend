namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO;
using Infrastructure.Repositories;
using MediatR;

public class InsuranceSearchRequest(string search) : IRequest<List<GetInsuranceDto>>
{
    public string Search { get; } = search;
}

public class InsuranceSearchRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper)
    : IRequestHandler<InsuranceSearchRequest, List<GetInsuranceDto>>
{
    public async Task<List<GetInsuranceDto>> Handle(InsuranceSearchRequest request, CancellationToken cancellationToken)
    {
        var insurances = await insuranceRepository.SearchInsurancesByNameAsync(request.Search);
        return mapper.Map<List<GetInsuranceDto>>(insurances);
    }
}