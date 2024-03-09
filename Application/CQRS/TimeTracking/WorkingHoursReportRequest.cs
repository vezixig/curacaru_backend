namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using AutoMapper;
using Core.DTO.TimeTracker;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class WorkingHoursReportRequest(
    Guid companyId,
    string authId,
    Guid employeeId,
    int month,
    int year) : IRequest<GetWorkingTimeReportDto>
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public Guid EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public int Year { get; } = year;
}

internal class WorkingHoursReportRequestHandler(IWorkingHoursRepository workingHoursRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    : IRequestHandler<WorkingHoursReportRequest, GetWorkingTimeReportDto>
{
    public async Task<GetWorkingTimeReportDto> Handle(WorkingHoursReportRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (!user!.IsManager && user.Id != request.EmployeeId) throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten abrufen");

        var reports = await workingHoursRepository.GetWorkingTimeReportsAsync(request.CompanyId, request.Year, request.Month, user.Id);
        var report = reports.SingleOrDefault() ?? throw new NotFoundException("Arbeitszeitnachweis nicht gefunden");

        return mapper.Map<GetWorkingTimeReportDto>(report);
    }
}