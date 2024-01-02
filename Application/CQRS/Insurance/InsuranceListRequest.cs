namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO.Insurance;
using Infrastructure.Repositories;
using MediatR;

public class InsuranceListRequest(Guid companyId) : IRequest<List<GetInsuranceDto>>
{
    public Guid CompanyId { get; } = companyId;
}

internal class InsuranceListRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper)
    : IRequestHandler<InsuranceListRequest, List<GetInsuranceDto>>
{
    public async Task<List<GetInsuranceDto>> Handle(InsuranceListRequest request, CancellationToken cancellationToken)
    {
        var insurances = await insuranceRepository.GetInsurancesAsync(request.CompanyId);
        return mapper.Map<List<GetInsuranceDto>>(insurances);
    }
}