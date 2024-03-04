namespace Curacaru.Backend.Application.CQRS.Company;

using Core.DTO.Company;
using Core.Exceptions;
using Infrastructure.repositories;
using MediatR;

public class UpdateCompanyRequest(Guid companyId, UpdateCompanyDto companyData) : IRequest
{
    public UpdateCompanyDto CompanyData { get; } = companyData;

    public Guid CompanyId { get; } = companyId;
}

internal class UpdateCompanyRequestHandler(ICompanyRepository companyRepository) : IRequestHandler<UpdateCompanyRequest>
{
    public async Task Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId)
                      ?? throw new NotFoundException("Company not found.");

        company.Bic = request.CompanyData.Bic;
        company.EmployeeSalary = request.CompanyData.EmployeeSalary;
        company.Iban = request.CompanyData.Iban;
        company.InstitutionCode = request.CompanyData.InstitutionCode;
        company.OwnerName = request.CompanyData.OwnerName;
        company.PricePerHour = request.CompanyData.PricePerHour;
        company.RecognitionDate = request.CompanyData.RecognitionDate;
        company.RideCosts = request.CompanyData.RideCosts;
        company.RideCostsType = request.CompanyData.RideCostsType;
        company.ServiceId = request.CompanyData.ServiceId;
        company.Street = request.CompanyData.Street;
        company.TaxNumber = request.CompanyData.TaxNumber;
        company.ZipCode = request.CompanyData.ZipCode;
        company.ZipCity = null;

        await companyRepository.UpdateCompanyAsync(company);
    }
}