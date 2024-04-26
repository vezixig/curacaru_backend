namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.AssignmentDeclaration;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request to add a new and signed assignment declaration.</summary>
/// <param name="document">The document data</param>
public class AddAssignmentDeclarationRequest(User user, AddAssignmentDeclarationDto document) : IRequest
{
    public AddAssignmentDeclarationDto Document { get; } = document;

    public User User { get; } = user;
}

internal class AddAssignmentDeclarationRequestHandler(
    ICustomerRepository customerRepository,
    IDocumentRepository documentRepository,
    IImageService imageService,
    IInsuranceRepository insuranceRepository) : IRequestHandler<AddAssignmentDeclarationRequest>
{
    public async Task Handle(AddAssignmentDeclarationRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerAsync(
                           request.User.CompanyId,
                           request.Document.CustomerId,
                           request.User.IsManager ? null : request.User.EmployeeId)
                       ?? throw new BadRequestException("Du darfst für diesen Kunden keine Abtretungserklärung erstellen.");

        if (string.IsNullOrEmpty(customer.InsuredPersonNumber)) throw new BadRequestException("Der Kunde hat keine Versichertennummer.");

        if (string.IsNullOrEmpty(customer.ZipCode)) throw new BadRequestException("Der Kunde hat keine Postleitzahl.");

        if (customer.InsuranceId is null || customer.InsuranceStatus != InsuranceStatus.Statutory)
            throw new BadRequestException("Der Kunde ist nicht gesetzlich versichert.");

        var insurance = await insuranceRepository.GetInsuranceAsync(request.User.CompanyId, customer.InsuranceId.Value)
                        ?? throw new BadRequestException("Die Versicherung konnte nicht gefunden werden.");

        if (string.IsNullOrEmpty(insurance.ZipCode)) throw new BadRequestException("Die Versicherung hat keine Postleitzahl.");

        if (await documentRepository.DoesAssignmentDeclarationExistAsync(request.Document.CustomerId, request.Document.Year))
            throw new BadRequestException("Für diesen Kunden existiert bereits eine Abtretungserklärung für das Jahr.");

        var document = new AssignmentDeclaration
        {
            CompanyId = request.User.CompanyId,
            Customer = customer,
            CustomerFirstName = customer.FirstName,
            CustomerId = request.Document.CustomerId,
            CustomerLastName = customer.LastName,
            CustomerStreet = customer.Street,
            CustomerZipCode = customer.ZipCode,
            CustomerZipCity = customer.ZipCity!,
            DateOfBirth = customer.BirthDate,
            InsuranceId = customer.InsuranceId.Value,
            InsuranceName = insurance.Name,
            InsuranceStreet = insurance.Street,
            InsuranceZipCode = insurance.ZipCode,
            InsuranceZipCity = insurance.ZipCity!,
            InsuredPersonNumber = customer.InsuredPersonNumber,
            Signature = imageService.ReduceImage(request.Document.Signature),
            SignatureCity = request.Document.SignatureCity,
            SignatureDate = DateOnly.FromDateTime(DateTime.Today),
            Year = request.Document.Year
        };
        await documentRepository.AddAssignmentDeclarationAsync(document);
    }
}