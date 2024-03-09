namespace Curacaru.Backend.Application.CQRS.TimeTracking;

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
        if (user!.Id != request.Data.EmployeeId) throw new UnauthorizedAccessException("Du darfst nur deine eigenen Arbeitszeiten unterschreiben");

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