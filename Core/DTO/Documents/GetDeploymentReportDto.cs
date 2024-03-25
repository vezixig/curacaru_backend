namespace Curacaru.Backend.Core.DTO.Documents;

public record GetDeploymentReportDto(
    bool IsCreated,
    bool HasUnfinishedAppointment,
    string EmployeeName,
    string ReplacementEmployeeNames,
    List<GetDeploymentReportTimeDto> Times,
    double TotalDuration)
{
}