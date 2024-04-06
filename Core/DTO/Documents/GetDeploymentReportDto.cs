namespace Curacaru.Backend.Core.DTO.Documents;

public record GetDeploymentReportDto(
    bool IsCreated,
    bool HasUnfinishedAppointment,
    string EmployeeName,
    string ReplacementEmployeeNames,
    Guid? ReportId,
    List<GetDeploymentReportTimeDto> Times,
    double TotalDuration,
    bool HasInvoice)
{
}