namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

/// <summary>Request for an assignment declaration.</summary>
/// <remarks>Needs to be signed once per year from every customer with statutory insurance.</remarks>
/// <param name="year">The year to create the declaration for.</param>
public class AssignmentDeclarationRequest(
    Guid companyId,
    string authId,
    Guid customerId,
    int year) : IRequest<byte[]>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid CustomerId { get; } = customerId;

    /// <summary>Gets the year to create the declaration for.</summary>
    public int Year { get; } = year;
}

internal class AssignmentDeclarationRequestHandler(
    ICompanyRepository companyRepository,
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IReportService reportService) : IRequestHandler<AssignmentDeclarationRequest, byte[]>
{
    public async Task<byte[]> Handle(AssignmentDeclarationRequest request, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetCompanyByIdAsync(request.CompanyId)
                      ?? throw new BadRequestException("Firma existiert nicht.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId)
                   ?? throw new BadRequestException("Mitarbeiter existiert nicht.");

        var customer = await customerRepository.GetCustomerAsync(request.CompanyId, request.CustomerId)
                       ?? throw new BadRequestException("Kunde existiert nicht.");

        if (customer.AssociatedEmployeeId != user.Id && !user.IsManager) throw new ForbiddenException("Diesen Kunden darfst du nicht bearbeiten.");

        return reportService.CreateAssignmentDeclaration(company, customer, request.Year);
    }
}