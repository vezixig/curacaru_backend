namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO;
using Core.DTO.Insurance;
using Infrastructure.Repositories;
using MediatR;

public class InsuranceListRequest(Guid companyId, int page, int pageSize) : IRequest<PageDto<GetInsuranceDto>>
{
    public Guid CompanyId { get; } = companyId;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;
}

internal class InsuranceListRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper)
    : IRequestHandler<InsuranceListRequest, PageDto<GetInsuranceDto>>
{
    public async Task<PageDto<GetInsuranceDto>> Handle(InsuranceListRequest request, CancellationToken cancellationToken)
    {
        var insuranceCount = await insuranceRepository.GetInsuranceCountAsync(request.CompanyId);
        var insurances = await insuranceRepository.GetInsurancesAsync(request.CompanyId, request.Page, request.PageSize);
        return new(mapper.Map<List<GetInsuranceDto>>(insurances), request.Page, (int)Math.Ceiling((double)insuranceCount / request.PageSize));
    }
}