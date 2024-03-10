﻿namespace Curacaru.Backend.Application.CQRS.TimeTracking;

using Core.DTO.TimeTracker;
using Core.Entities;
using Infrastructure.repositories;
using Infrastructure.Repositories;
using MediatR;

public class AddWorkingTimeSignatureRequest(
    Guid companyId,
    string authId,
    AddWorkTimeSignatureDto data) : IRequest
{
    public string AuthId { get; } = authId;

    public Guid CompanyId { get; } = companyId;

    public AddWorkTimeSignatureDto Data { get; } = data;
}

internal class AddWorkingTimeSignatureRequestHandler(
    IAppointmentRepository appointmentRepository,
    IEmployeeRepository employeeRepository,
    IWorkingHoursRepository workingHoursRepository)
    : IRequestHandler<AddWorkingTimeSignatureRequest>
{
    public async Task Handle(AddWorkingTimeSignatureRequest request, CancellationToken cancellationToken)
    {
        var user = await employeeRepository.GetEmployeeByAuthIdAsync(request.AuthId);
        if (user!.Id != request.Data.EmployeeId && !user.IsManager)
            throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten unterschreiben");

        var existingReport = await workingHoursRepository.GetWorkingTimeReportsAsync(
            request.CompanyId,
            request.Data.Year,
            request.Data.Month,
            request.Data.EmployeeId);

        if ((existingReport.Any() && !user.IsManager) || existingReport[0].SignatureManagerDate is not null)
            throw new InvalidOperationException("Du hast diesen Bericht bereits unterschrieben");

        if (existingReport.Count > 0)
        {
            existingReport[0].SignatureManager = request.Data.Signature;
            existingReport[0].SignatureManagerCity = request.Data.SignatureCity;
            existingReport[0].SignatureManagerDate = request.Data.SignatureDate;

            await workingHoursRepository.UpdateWorkingTimeReportAsync(existingReport[0]);
        }
        else
        {
            // /calculate working hours
            var start = new DateOnly(request.Data.Year, request.Data.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            var appointments = await appointmentRepository.GetAppointmentsAsync(request.CompanyId, start, end, user.Id, null);
            var totalHours = appointments.Sum(o => (o.TimeEnd - o.TimeStart).TotalHours);

            var report = new WorkingTimeReport
            {
                CompanyId = request.CompanyId,
                EmployeeId = user.Id,
                Month = request.Data.Month,
                Year = request.Data.Year,
                SignatureEmployee = request.Data.Signature,
                SignatureEmployeeCity = request.Data.SignatureCity,
                SignatureEmployeeDate = request.Data.SignatureDate,
                TotalHours = totalHours
            };

            await workingHoursRepository.AddWorkingTimeReportAsync(report);
        }
    }
}