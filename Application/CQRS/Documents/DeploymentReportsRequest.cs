namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.Documents;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class DeploymentReportsRequest(
    Guid companyId,
    string authId,
    int year,
    int month,
    Guid? customerId,
    Guid? employeeId) : IRequest<List<GetDeploymentReportListEntryDto>>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class DeploymentReportsRequestHandler(
    IAppointmentRepository appointmentRepository,
    IEmployeeRepository employeeRepository,
    IDocumentRepository documentRepository)
    : IRequestHandler<DeploymentReportsRequest, List<GetDeploymentReportListEntryDto>>
{
    public async Task<List<GetDeploymentReportListEntryDto>> Handle(DeploymentReportsRequest request, CancellationToken cancellationToken)
    {
        if (request.Year >= DateTime.Now.Year && request.Month > DateTime.Now.Month) return [];

        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var possibleReports = await appointmentRepository.GetClearanceTypes(
            request.CompanyId,
            request.CustomerId,
            request.EmployeeId,
            request.Year,
            request.Month);

        var reports = await documentRepository.GetDeploymentReportsAsync(
            request.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month);

        if (!user!.IsManager)
            possibleReports = possibleReports.Where(o => o.Employees.Exists(p => p.Id == user.Id) || o.ReplacementEmployee.Exists(p => p.Id == user.Id)).ToList();

        return possibleReports.Select(
                o => new GetDeploymentReportListEntryDto(
                    reports.Exists(r => r.CustomerId == o.Customer.Id && r.ClearanceType == o.ClearanceType),
                    reports.Find(r => r.CustomerId == o.Customer.Id && r.ClearanceType == o.ClearanceType)?.Id,
                    o.ClearanceType,
                    o.Customer.Id,
                    request.Month,
                    request.Year,
                    o.Customer.FullName,
                    string.Join(", ", o.Employees.Select(p => p.FullName).Distinct()),
                    string.Join(", ", o.ReplacementEmployee.Select(p => p.FullName).Distinct())))
            .ToList();
    }
}