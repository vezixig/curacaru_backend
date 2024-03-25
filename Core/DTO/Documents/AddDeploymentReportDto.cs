namespace Curacaru.Backend.Core.DTO.Documents;

using Enums;

/// <summary>Data transfer object for adding a deployment report.</summary>
public record AddDeploymentReportDto(
    ClearanceType ClearanceType,
    Guid CustomerId,
    int Month,
    string SignatureCity,
    string SignatureCustomer,
    string SignatureEmployee,
    int Year)
{
}