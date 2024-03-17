namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class AddWorkingTimeSignatureRequest(
    Guid companyId,
    string authId,
    AddWorkingTimeReportSignatureDto data) : IRequest
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public AddWorkingTimeReportSignatureDto Data { get; } = data;
}

internal class AddWorkingTimeSignatureRequestHandler(
    IAppointmentRepository appointmentRepository,
    IEmployeeRepository employeeRepository,
    IWorkingTimeRepository workingTimeRepository)
    : IRequestHandler<AddWorkingTimeSignatureRequest>
{
    public async Task Handle(AddWorkingTimeSignatureRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != request.Data.EmployeeId && !user.IsManager)
            throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten unterschreiben");

        var existingReport = await workingTimeRepository.GetWorkingTimeReportsAsync(
            request.CompanyId,
            request.Data.Year,
            request.Data.Month,
            request.Data.EmployeeId);

        if (existingReport.Any() && (!user.IsManager || existingReport[0].SignatureManagerDate is not null))
            throw new InvalidOperationException("Du hast diesen Bericht bereits unterschrieben");

        if (existingReport.Count > 0)
        {
            existingReport[0].SignatureManager = request.Data.Signature;
            existingReport[0].SignatureManagerCity = request.Data.SignatureCity;
            existingReport[0].SignatureManagerDate = DateOnly.FromDateTime(DateTime.Today);

            await workingTimeRepository.UpdateWorkingTimeReportAsync(existingReport[0]);
        }
        else
        {
            // /calculate working hours
            var start = new DateOnly(request.Data.Year, request.Data.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, start, end, user.Id, null);
            appointments = appointments.Where(o => o.EmployeeReplacementId == user.Id || o.EmployeeReplacementId is null).ToList();

            if (appointments.Count == 0) throw new BadRequestException("Es gibt keine Arbeitszeiten in diesem Monat");
            if (appointments.Exists(o => !o.IsDone)) throw new BadRequestException("Es gibt noch nicht abgeschlossene Termine in diesem Monat");

            var totalHours = appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours);

            var report = new WorkingTimeReport
            {
                CompanyId = request.CompanyId,
                EmployeeId = user.Id,
                Month = request.Data.Month,
                Year = request.Data.Year,
                SignatureEmployee = request.Data.Signature,
                SignatureEmployeeCity = request.Data.SignatureCity,
                SignatureEmployeeDate = DateOnly.FromDateTime(DateTime.Today),
                TotalHours = totalHours
            };

            await workingTimeRepository.AddWorkingTimeReportAsync(report);
        }
    }
}