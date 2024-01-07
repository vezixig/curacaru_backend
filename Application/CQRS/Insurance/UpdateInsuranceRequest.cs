namespace Curacaru.Backend.Application.CQRS.Insurance;

using AutoMapper;
using Core.DTO.Insurance;
using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class UpdateInsuranceRequest(Guid companyId, UpdateInsuranceDto insurance) : IRequest<GetInsuranceDto>
{
    public Guid CompanyId { get; } = companyId;

    public UpdateInsuranceDto Insurance { get; } = insurance;
}

internal class UpdateInsuranceRequestHandler(IInsuranceRepository insuranceRepository, IMapper mapper) : IRequestHandler<UpdateInsuranceRequest, GetInsuranceDto>
{
    public async Task<GetInsuranceDto> Handle(UpdateInsuranceRequest request, CancellationToken cancellationToken)
    {
        var insurance = await insuranceRepository.GetInsuranceAsync(request.CompanyId, request.Insurance.Id)
                        ?? throw new NotFoundException("Versicherung wurde nicht gefunden");

        insurance.InstitutionCode = request.Insurance.InstitutionCode;
        insurance.Name = request.Insurance.Name;
        insurance.Street = request.Insurance.Street;
        insurance.ZipCode = request.Insurance.ZipCode;

        insurance = await insuranceRepository.UpdateInsuranceAsync(insurance);
        return mapper.Map<GetInsuranceDto>(insurance);
    }
}