namespace Curacaru.Backend.Application.CQRS.Invoices;

using Core.DTO.Invoice;
using Core.Entities;
using Core.Enums;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class AddInvoiceRequest(Guid companyId, Guid userId, AddInvoiceDto invoice) : IRequest
{
    public Guid CompanyId { get; } = companyId;

    public AddInvoiceDto Invoice { get; } = invoice;

    public Guid UserId { get; } = userId;
}

internal class AddInvoiceRequestHandler(
    ICompanyRepository companyRepository,
    IDocumentRepository documentRepository,
    IEmployeeRepository employeeRepository,
    IImageService imageService,
    IInvoiceRepository invoiceRepository)
    : IRequestHandler<AddInvoiceRequest>
{
    public async Task Handle(AddInvoiceRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByIdAsync(request.CompanyId, request.UserId);

        var deploymentReport = await documentRepository.GetDeploymentReportByIdAsync(request.CompanyId, request.Invoice.DeploymentReportId)
                               ?? throw new BadRequestException("Der Einsatzbericht wurde nicht gefunden.");

        if (deploymentReport.Invoice is not null) throw new BadRequestException("Die Rechnung wurde bereits erfasst.");

        var companyData = await companyRepository.GetCompanyByIdAsync(request.CompanyId);

        var rideCosts = companyData!.RideCostsType switch
        {
            RideCostsType.None => 0,
            RideCostsType.Inclusive => 0,
            RideCostsType.FlatRate => deploymentReport.Appointments.Count * companyData.RideCosts,
            RideCostsType.Kilometer => deploymentReport.Appointments.Sum(o => o.DistanceToCustomer * companyData.RideCosts),
            _ => 0
        };

        var invoice = new Invoice
        {
            CompanyId = request.CompanyId,
            DeploymentReport = deploymentReport,
            DeploymentReportId = deploymentReport.Id,
            HourlyRate = companyData.PricePerHour,
            InvoiceDate = request.Invoice.InvoiceDate,
            InvoiceNumber = request.Invoice.InvoiceNumber,
            RideCosts = companyData.RideCosts,
            RideCostsType = companyData.RideCostsType,
            Signature = imageService.ReduceImage(request.Invoice.Signature),
            SignedEmployee = user!,
            SignedEmployeeId = request.UserId,
            TotalRideCosts = rideCosts,
            WorkedHours = (decimal)deploymentReport.Appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours)
        };
        invoice.InvoiceTotal = invoice.WorkedHours * invoice.HourlyRate + invoice.TotalRideCosts;

        await invoiceRepository.AddInvoiceAsync(invoice);
    }
}