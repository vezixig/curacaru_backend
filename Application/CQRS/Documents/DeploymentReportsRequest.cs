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
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);

        var employeeId = user!.IsManager ? request.EmployeeId : user.Id;
        var possibleReports = await appointmentRepository.GetClearanceTypes(
            request.CompanyId,
            request.CustomerId,
            employeeId,
            request.Year,
            request.Month);

        var reports = await documentRepository.GetDeploymentReportsAsync(
            request.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month);

        return possibleReports.Select(
                o => new GetDeploymentReportListEntryDto(
                    reports.Exists(r => r.CustomerId == o.Customer.Id && r.ClearanceType == o.ClearanceType),
                    o.ClearanceType,
                    o.Customer.Id,
                    o.Employee.Id,
                    request.Month,
                    request.Year,
                    o.Customer.FullName,
                    o.Employee.FullName,
                    string.Join(", ", o.ReplacementEmployee.Select(p => p.FullName))))
            .ToList();
    }
}