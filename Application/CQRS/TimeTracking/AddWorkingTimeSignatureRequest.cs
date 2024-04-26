namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;

public class AddWorkingTimeSignatureRequest(
    User user,
    AddWorkingTimeReportSignatureDto data) : IRequest
{
    public AddWorkingTimeReportSignatureDto Data { get; } = data;

    public User User { get; } = user;
}

internal class AddWorkingTimeSignatureRequestHandler(
    IAppointmentRepository appointmentRepository,
    IImageService imageService,
    IWorkingTimeRepository workingTimeRepository)
    : IRequestHandler<AddWorkingTimeSignatureRequest>
{
    public async Task Handle(AddWorkingTimeSignatureRequest request, CancellationToken cancellationToken)
    {
        if (request.User.EmployeeId != request.Data.EmployeeId && !request.User.IsManager)
            throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten unterschreiben");

        var existingReport = await workingTimeRepository.GetWorkingTimeReportsAsync(
            request.User.CompanyId,
            request.Data.Year,
            request.Data.Month,
            request.Data.EmployeeId);

        if (existingReport.Any() && (!request.User.IsManager || existingReport[0].SignatureManagerDate is not null))
            throw new InvalidOperationException("Du hast diesen Bericht bereits unterschrieben");

        if (existingReport.Count > 0)
        {
            existingReport[0].SignatureManager = imageService.ReduceImage(request.Data.Signature);
            existingReport[0].SignatureManagerCity = request.Data.SignatureCity;
            existingReport[0].SignatureManagerDate = DateOnly.FromDateTime(DateTime.Today);

            await workingTimeRepository.UpdateWorkingTimeReportAsync(existingReport[0]);
        }
        else
        {
            // /calculate working hours
            var start = new DateOnly(request.Data.Year, request.Data.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            var appointments = await appointmentRepository.GetAppointmentsAsync(request.User.CompanyId, start, end, request.User.EmployeeId, null);
            appointments = appointments.Where(o => o.EmployeeReplacementId == request.User.EmployeeId || o.EmployeeReplacementId is null).ToList();

            if (appointments.Count == 0) throw new BadRequestException("Es gibt keine Arbeitszeiten in diesem Monat");
            if (appointments.Exists(o => !o.IsDone)) throw new BadRequestException("Es gibt noch nicht abgeschlossene Termine in diesem Monat");

            var totalHours = appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours);

            var report = new WorkingTimeReport
            {
                CompanyId = request.User.CompanyId,
                EmployeeId = request.User.EmployeeId,
                Month = request.Data.Month,
                Year = request.Data.Year,
                SignatureEmployee = imageService.ReduceImage(request.Data.Signature),
                SignatureEmployeeCity = request.Data.SignatureCity,
                SignatureEmployeeDate = DateOnly.FromDateTime(DateTime.Today),
                TotalHours = totalHours
            };

            await workingTimeRepository.AddWorkingTimeReportAsync(report);
        }
    }
}