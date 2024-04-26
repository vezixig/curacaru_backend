namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO.Documents;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class DeploymentReportsRequest(
    User user,
    int year,
    int month,
    Guid? customerId,
    Guid? employeeId) : IRequest<List<GetDeploymentReportListEntryDto>>
{
    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class DeploymentReportsRequestHandler(
    IAppointmentRepository appointmentRepository,
    IDocumentRepository documentRepository)
    : IRequestHandler<DeploymentReportsRequest, List<GetDeploymentReportListEntryDto>>
{
    public async Task<List<GetDeploymentReportListEntryDto>> Handle(DeploymentReportsRequest request, CancellationToken cancellationToken)
    {
        if (request.Year >= DateTime.Now.Year && request.Month > DateTime.Now.Month) return [];

        var possibleReports = await appointmentRepository.GetClearanceTypes(
            request.User.CompanyId,
            request.CustomerId,
            request.EmployeeId,
            request.Year,
            request.Month);

        var reports = await documentRepository.GetDeploymentReportsAsync(
            request.User.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month);

        if (!request.User.IsManager)
            possibleReports = possibleReports.Where(
                    o => o.Employees.Exists(p => p.Id == request.User.EmployeeId) || o.ReplacementEmployee.Exists(p => p.Id == request.User.EmployeeId))
                .ToList();

        return possibleReports.Select(
                o => new GetDeploymentReportListEntryDto(
                    reports.Exists(r => r.CustomerId == o.Customer.Id && r.ClearanceType == o.ClearanceType),
                    reports.Find(r => r.CustomerId == o.Customer.Id && r.ClearanceType == o.ClearanceType)?.Id,
                    o.ClearanceType,
                    o.Customer.Id,
                    request.Month,
                    request.Year,
                    o.Customer.FullNameReverse,
                    string.Join(", ", o.Employees.Select(p => p.FullName).Distinct()),
                    string.Join(", ", o.ReplacementEmployee.Select(p => p.FullName).Distinct())))
            .OrderBy(o => o.CustomerName)
            .ToList();
    }
}