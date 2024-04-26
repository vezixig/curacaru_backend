namespace Curacaru.Backend.Application.CQRS.Company;

using AutoMapper;
using Core.DTO.Company;
using Infrastructure.Repositories;
using MediatR;

/// <summary>Request for a company by its id.</summary>
/// <param name="companyId">The company id.</param>
public class CompanyRequest(Guid companyId) : IRequest<GetCompanyDto?>
{
    public Guid CompanyId { get; } = companyId;
}

internal class CompanyRequestHandler(ICompanyRepository companyRepository, IMapper mapper) : IRequestHandler<CompanyRequest, GetCompanyDto?>
{
    public async Task<GetCompanyDto?> Handle(CompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId);
        return company == null ? null : mapper.Map<GetCompanyDto>(company);
    }
}