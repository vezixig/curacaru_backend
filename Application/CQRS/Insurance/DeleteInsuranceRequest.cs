namespace Curacaru.Backend.Application.CQRS.Insurance;

using Core.Exceptions;
using Infrastructure.Repositories;
using MediatR;

public class DeleteInsuranceRequest(Guid companyId, Guid insuranceId) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public Guid InsuranceId { get; } = insuranceId;
}

internal class DeleteInsuranceRequestHandler(IInsuranceRepository insuranceRepository) : IRequestHandler<DeleteInsuranceRequest>
{
    public async Task Handle(DeleteInsuranceRequest request, CancellationToken cancellationToken)
    {
        var insurance = await insuranceRepository.GetInsuranceAsync(request.CompanyId, request.InsuranceId)
                        ?? throw new NotFoundException("Versicherung wurde nicht gefunden");
        await insuranceRepository.DeleteInsuranceAsync(insurance);
    }
}