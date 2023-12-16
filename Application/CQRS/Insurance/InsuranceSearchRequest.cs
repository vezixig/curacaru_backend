namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO;
using Infrastructure.Repositories;
using MediatR;

public class InsuranceSearchRequest : IRequest<List<GetInsuranceDto>>
{
    public InsuranceSearchRequest(string search)
        => Search = search;

    public string Search { get; }
}

public class InsuranceSearchRequestHandler : IRequestHandler<InsuranceSearchRequest, List<GetInsuranceDto>>
{
    private readonly IInsuranceRepository _insuranceRepository;

    private readonly IMapper _mapper;

    public InsuranceSearchRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper)
    {
        _insuranceRepository = insuranceRepository;
        _mapper = mapper;
    }

    public async Task<List<GetInsuranceDto>> Handle(InsuranceSearchRequest request, CancellationToken cancellationToken)
    {
        var insurances = await _insuranceRepository.SearchInsurancesByNameAsync(request.Search);
        return _mapper.Map<List<GetInsuranceDto>>(insurances);
    }
}