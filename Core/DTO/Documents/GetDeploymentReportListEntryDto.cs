﻿namespace Curacaru.Backend.Core.DTO.Documents;

using Enums;

public record GetDeploymentReportListEntryDto(
    bool IsCreated,
    Guid? ReportId,
    ClearanceType ClearanceType,
    Guid CustomerId,
    int Month,
    int Year,
    string CustomerName,
    string EmployeeName,
    string ReplacementEmployeeNames)
{
}