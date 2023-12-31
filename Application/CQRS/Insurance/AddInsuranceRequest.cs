namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO.Insurance;
using Core.Entities;
using Infrastructure.Repositories;
using MediatR;

public class AddInsuranceRequest(Guid companyId, AddInsuranceDto insurance) : IRequest<GetInsuranceDto>

{
    public Guid CompanyId { get; } = companyId;

    public AddInsuranceDto Insurance { get; } = insurance;
}

internal class AddInsuranceRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper) : IRequestHandler<AddInsuranceRequest, GetInsuranceDto>
{
    public async Task<GetInsuranceDto> Handle(AddInsuranceRequest request, CancellationToken cancellationToken)
    {
        var insurance = new Insurance
        {
            CompanyId = request.CompanyId,
            InstitutionCode = request.Insurance.InstitutionCode,
            Name = request.Insurance.Name
        };
        await insuranceRepository.AddInsuranceAsync(insurance);
        return mapper.Map<GetInsuranceDto>(insurance);
    }
}