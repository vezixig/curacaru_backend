namespace Curacaru.Backend.Core.DTO.Documents;

public record GetDeploymentReportTimeDto(
    Guid AppointmentId,
    DateOnly Date,
    TimeOnly Start,
    TimeOnly End,
    double Duration,
    decimal Distance,
    bool IsPlanned,
    bool IsDone)
{
}