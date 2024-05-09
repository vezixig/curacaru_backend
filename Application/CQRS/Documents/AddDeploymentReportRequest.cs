namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.Documents;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using DeploymentReport = Core.Entities.DeploymentReport;

public class AddDeploymentReportRequest(User user, AddDeploymentReportDto report) : IRequest
{
    public AddDeploymentReportDto Report { get; } = report;

    public User User { get; } = user;
}

internal class AddDeploymentReportRequestHandler(
    IAppointmentRepository appointmentRepository,
    ICustomerRepository customerRepository,
    IDocumentRepository documentRepository,
    IImageService imageService) : IRequestHandler<AddDeploymentReportRequest>
{
    public async Task Handle(AddDeploymentReportRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Report.SignatureEmployee) || string.IsNullOrEmpty(request.Report.SignatureCustomer))
            throw new BadRequestException("Es fehlt eine Unterschrift.");

        var start = new DateOnly(request.Report.Year, request.Report.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        var appointments = await appointmentRepository.GetAppointmentsAsync(
            request.User.CompanyId,
            start,
            end,
            null,
            request.Report.CustomerId,
            clearanceType: request.Report.ClearanceType,
            asTracking: true);
        appointments = appointments.FindAll(o => o.IsDone);

        if (!appointments.Exists(o => o.EmployeeId == request.User.EmployeeId || o.EmployeeReplacementId == request.User.EmployeeId))
            throw new ForbiddenException("Du darfst keine Berichte für diesen Kunden erstellen.");

        var customer = await customerRepository.GetCustomerAsync(request.User.CompanyId, request.Report.CustomerId, null, true)
                       ?? throw new BadRequestException("Kunde existiert nicht.");

        var deploymentReport = new DeploymentReport
        {
            Appointments = appointments,
            CareLevel = customer.CareLevel,
            ClearanceType = request.Report.ClearanceType,
            CompanyId = request.User.CompanyId,
            Customer = appointments[0].Customer,
            CustomerId = request.Report.CustomerId,
            CustomerInsuranceStatus = customer.InsuranceStatus,
            Insurance = customer.Insurance,
            InsuranceId = customer.InsuranceId,
            InsuredPersonNumber = customer.InsuredPersonNumber,
            Month = request.Report.Month,
            SignatureCity = request.Report.SignatureCity,
            SignatureCustomer = imageService.ReduceImage(request.Report.SignatureCustomer),
            SignatureDate = DateOnly.FromDateTime(DateTime.Today),
            SignatureEmployee = imageService.ReduceImage(request.Report.SignatureEmployee),
            WorkedHours = appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours),
            Year = request.Report.Year
        };

        await documentRepository.AddDeploymentReportAsync(deploymentReport);
    }
}