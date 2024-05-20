namespace Curacaru.Backend.Application.CQRS.Documents;

using Core.DTO;
using Core.DTO.Documents;
using Core.Models;
using Infrastructure.Repositories;
using MediatR;

public class DeploymentReportsRequest(
    User user,
    int year,
    int month,
    Guid? customerId,
    Guid? employeeId,
    int page,
    int pageSize) : IRequest<PageDto<GetDeploymentReportListEntryDto>>
{
    public Guid? CustomerId { get; } = customerId;

    public Guid? EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public int Page { get; } = page;

    public int PageSize { get; } = pageSize;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class DeploymentReportsRequestHandler(
    IAppointmentRepository appointmentRepository,
    IDocumentRepository documentRepository)
    : IRequestHandler<DeploymentReportsRequest, PageDto<GetDeploymentReportListEntryDto>>
{
    public async Task<PageDto<GetDeploymentReportListEntryDto>> Handle(DeploymentReportsRequest request, CancellationToken cancellationToken)
    {
        if (request.Year >= DateTime.Now.Year && request.Month > DateTime.Now.Month) return new([], 1, request.PageSize);
        var employeeId = request.User.IsManager ? request.EmployeeId : request.User.EmployeeId;

        var reportCount = await appointmentRepository.GetClearanceTypesCount(
            request.User.CompanyId,
            request.CustomerId,
            employeeId,
            request.Year,
            request.Month);
        var possibleReports = await appointmentRepository.GetClearanceTypes(
            request.User.CompanyId,
            request.CustomerId,
            employeeId,
            request.Year,
            request.Month,
            request.Page,
            request.PageSize);

        var reports = await documentRepository.GetDeploymentReportsAsync(
            request.User.CompanyId,
            request.CustomerId,
            request.Year,
            request.Month);

        if (!request.User.IsManager)
            possibleReports = possibleReports.Where(
                    o => o.Employees.Exists(p => p.Id == request.User.EmployeeId) || o.ReplacementEmployee.Exists(p => p.Id == request.User.EmployeeId))
                .ToList();

        return new(
            possibleReports.Select(
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
                .ToList(),
            request.Page,
            (int)Math.Ceiling((decimal)reportCount / request.PageSize));
    }
}