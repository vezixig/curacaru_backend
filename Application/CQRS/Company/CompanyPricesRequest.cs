namespace Curacaru.Backend.Application.CQRS.Company;

using AutoMapper;
using Core.DTO.Company;
using Infrastructure.repositories;
using MediatR;

/// <summary>Request for a company by its id.</summary>
/// <param name="companyId">The company id.</param>
public class CompanyPricesRequest(Guid companyId) : IRequest<GetCompanyPricesDto>
{
    public Guid CompanyId { get; } = companyId;
}

internal class CompanyPricesRequestHandler(ICompanyRepository companyRepository, IMapper mapper) : IRequestHandler<CompanyPricesRequest, GetCompanyPricesDto>
{
    public async Task<GetCompanyPricesDto> Handle(CompanyPricesRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);
        return mapper.Map<GetCompanyPricesDto>(company);
    }
}