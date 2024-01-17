﻿namespace Curacaru.Backend.Infrastructure.Services;

using Core.Entities;
using Core.Enums;

/// <summary>Service to create reports.</summary>
public interface IReportService
{
    /// <summary>Creates a deployment report.</summary>
    byte[] CreateDeploymentReport(Company company, Customer customer, InsuranceStatus insuranceStatus);
}