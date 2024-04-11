namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.Documents;
using Core.Enums;
using Infrastructure.Repositories;
using MediatR;

public class DeploymentReportRequest(
    Guid companyId,
    string authId,
    Guid customerId,
    int year,
    int month,
    ClearanceType clearanceType) : IRequest<GetDeploymentReportDto?>
{
    public string AuthId { get; } = authId;

    public ClearanceType ClearanceType { get; } = clearanceType;

    public Guid CompanyId { get; } = companyId;

    public Guid CustomerId { get; } = customerId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class DeploymentReportRequestHandler(IAppointmentRepository appointmentRepository, IDocumentRepository documentRepository)
    : IRequestHandler<DeploymentReportRequest, GetDeploymentReportDto?>
{
    public async Task<GetDeploymentReportDto?> Handle(DeploymentReportRequest request, CancellationToken cancellationToken)
    {
        if (request.Year >= DateTime.Now.Year && request.Month > DateTime.Now.Month) return null;

        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, start, end, null, request.CustomerId, request.ClearanceType);

        if (appointments.Count == 0) return null;

        appointments = appointments.OrderBy(o => o.Date).ThenBy(o => o.TimeStart).ToList();

        var deploymentReport = await documentRepository.GetDeploymentReportsAsync(
            request.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month,
            request.ClearanceType);

        return new(
            EmployeeName: string.Join(", ", appointments.Select(o => o.Employee.FullName).Distinct()),
            IsCreated: deploymentReport.Count == 1,
            HasUnfinishedAppointment: appointments.Exists(o => !o.IsDone && !o.IsPlanned),
            HasPlannedAppointment: appointments.Exists(o => o.IsPlanned),
            ReplacementEmployeeNames: string.Join(
                ", ",
                appointments.Where(o => o.EmployeeReplacement != null).Select(o => o.EmployeeReplacement!.FullName).Distinct()),
            ReportId: deploymentReport.FirstOrDefault()?.Id,
            Times: appointments.Select(
                    o => new GetDeploymentReportTimeDto(
                        o.Id,
                        IsDone: o.IsDone,
                        Date: o.Date,
                        Start: o.TimeStart,
                        End: o.TimeEnd,
                        Duration: (o.TimeEnd - o.TimeStart).TotalHours,
                        Distance: o.DistanceToCustomer,
                        IsPlanned: o.IsPlanned))
                .ToList(),
            TotalDuration: appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours),
            HasInvoice: deploymentReport.FirstOrDefault()?.Invoice is not null
        );
    }
}