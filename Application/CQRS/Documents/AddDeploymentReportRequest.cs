namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.Documents;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class AddDeploymentReportRequest(Guid companyId, string authId, AddDeploymentReportDto report) : IRequest
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public AddDeploymentReportDto Report { get; } = report;
}

internal class AddDeploymentReportRequestHandler(
    IAppointmentRepository appointmentRepository,
    IDocumentRepository documentRepository,
    IEmployeeRepository employeeRepository,
    IImageService imageService) : IRequestHandler<AddDeploymentReportRequest>
{
    public async Task Handle(AddDeploymentReportRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Report.SignatureEmployee) || string.IsNullOrEmpty(request.Report.SignatureCustomer))
            throw new BadRequestException("Es fehlt eine Unterschrift.");

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        var start = new DateOnly(request.Report.Year, request.Report.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        var appointments = await appointmentRepository.GetAppointmentsAsync(
            request.CompanyId,
            start,
            end,
            null,
            request.Report.CustomerId,
            request.Report.ClearanceType,
            true);
        appointments = appointments.FindAll(o => o.IsDone);

        if (!appointments.Exists(o => o.EmployeeId == user!.Id || o.EmployeeReplacementId == user.Id))
            throw new ForbiddenException("Du darfst keine Berichte für diesen Kunden erstellen.");

        var deploymentReport = new DeploymentReport
        {
            CompanyId = request.CompanyId,
            CustomerId = request.Report.CustomerId,
            Year = request.Report.Year,
            Month = request.Report.Month,
            ClearanceType = request.Report.ClearanceType,
            SignatureCity = request.Report.SignatureCity,
            SignatureCustomer = imageService.ReduceImage(request.Report.SignatureCustomer),
            SignatureEmployee = imageService.ReduceImage(request.Report.SignatureEmployee),
            Customer = appointments[0].Customer,
            WorkedHours = appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours),
            SignatureDate = new(request.Report.Year, request.Report.Month, 1),
            Appointments = appointments
        };

        await documentRepository.AddDeploymentReportAsync(deploymentReport);
    }
}