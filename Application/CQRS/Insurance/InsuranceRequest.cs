namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO.Insurance;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for an insurance.</summary>
/// <param name="companyId">The company id.</param>
/// <param name="insuranceId">The insurance id.</param>
public class InsuranceRequest(Guid companyId, Guid insuranceId) : IRequest<GetInsuranceDto?>
{
    public Guid CompanyId { get; } = companyId;

    public Guid InsuranceId { get; } = insuranceId;
}

internal class InsuranceRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper) : IRequestHandler<InsuranceRequest, GetInsuranceDto?>
{
    public async Task<GetInsuranceDto?> Handle(InsuranceRequest request, CancellationToken cancellationToken)
    {
        var insurance = await insuranceRepository.GetInsuranceAsync(request.CompanyId, request.InsuranceId);
        return insurance is null ? null : mapper.Map<GetInsuranceDto>(insurance);
    }
}