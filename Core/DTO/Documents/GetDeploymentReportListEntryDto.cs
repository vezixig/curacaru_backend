﻿namespace Curacaru.Backend.Core.DTO.Documents;

using Enums;

public record GetDeploymentReportListEntryDto(
    bool IsCreated,
    ClearanceType ClearanceType,
    Guid CustomerId,
    Guid EmployeeId,
    int Month,
    int Year,
    string CustomerName,
    string EmployeeName,
    string ReplacementEmployeeNames)
{
}