namespace Curacaru.Backend.Core.DTO.Documents;

public record GetDeploymentReportTimeDto(
    DateOnly Date,
    TimeOnly Start,
    TimeOnly End,
    double Duration)
{
}