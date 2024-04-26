namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class WorkingHoursReportPrintRequest(
    User user,
    Guid employeeId,
    int month,
    int year) : IRequest<byte[]>
{
    public Guid EmployeeId { get; } = employeeId;

    public int Month { get; } = month;

    public User User { get; } = user;

    public int Year { get; } = year;
}

internal class WorkingHoursReportPrintRequestHandler(
    IAppointmentRepository appointmentRepository,
    IWorkingTimeRepository workingTimeRepository,
    IReportService reportService)
    : IRequestHandler<WorkingHoursReportPrintRequest, byte[]>
{
    public async Task<byte[]> Handle(WorkingHoursReportPrintRequest request, CancellationToken cancellationToken)
    {
        if (!request.User.IsManager && request.User.EmployeeId != request.EmployeeId)
            throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeitreporte abrufen");

        var reports = await workingTimeRepository.GetWorkingTimeReportsAsync(request.User.CompanyId, request.Year, request.Month, request.EmployeeId);

        if (reports.Count == 0) throw new NotFoundException("Der Arbeitszeitreport für diese Monat wurde noch nicht unterschrieben.");
        if (reports[0].SignatureManagerDate is null) throw new BadRequestException("Der Report wurde noch nicht vom Manager unterschrieben");

        var start = new DateOnly(request.Year, request.Month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        var appointments = await appointmentRepository.GetAppointmentsAsync(request.User.CompanyId, start, end, request.EmployeeId, null);
        appointments = appointments.Where(o => o.EmployeeReplacementId == request.EmployeeId || o.EmployeeReplacementId is null).ToList();

        return reportService.GenerateWorkingHoursReport(reports[0], appointments);
    }
}