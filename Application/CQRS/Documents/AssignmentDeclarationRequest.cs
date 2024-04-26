namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request for an assignment declaration.</summary>
/// <remarks>Needs to be signed once per year from every customer with statutory insurance.</remarks>
/// <param name="year">The year to create the declaration for.</param>
public class AssignmentDeclarationRequest(
    User user,
    Guid customerId,
    int year) : IRequest<byte[]>
{
    public Guid CustomerId { get; } = customerId;

    public User User { get; } = user;

    /// <summary>Gets the year to create the declaration for.</summary>
    public int Year { get; } = year;
}

internal class AssignmentDeclarationRequestHandler(
    ICompanyRepository companyRepository,
    IDocumentRepository documentRepository,
    ICustomerRepository customerRepository,
    IReportService reportService) : IRequestHandler<AssignmentDeclarationRequest, byte[]>
{
    public async Task<byte[]> Handle(AssignmentDeclarationRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetCompanyByIdAsync(request.User.CompanyId);

        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.CustomerId)
                       ?? throw new BadRequestException("Kunde existiert nicht.");

        var document = await documentRepository.GetAssignmentDeclarationAsync(request.Year, request.CustomerId)
                       ?? throw new BadRequestException("Abtretungserklärung existiert nicht.");

        if (customer.AssociatedEmployeeId != request.User.CompanyId && !request.User.IsManager)
            throw new ForbiddenException("Diesen Kunden darfst du nicht bearbeiten.");

        return reportService.CreateAssignmentDeclaration(company!, document);
    }
}