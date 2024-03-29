﻿namespace Curacaru.Backend.Core.DTO.Customer;

using Enums;

/// <summary>A DTO to get a deployment.</summary>
public class GetMinimalCustomerListEntryDto
{
    /// <summary>Gets or sets the id of the customer.</summary>
    public Guid CustomerId { get; init; }

    /// <summary>Gets or sets the name of the customer.</summary>
    public string CustomerName { get; init; } = "";

    /// <summary>Gets or sets the insurance status of the customer.</summary>
    public InsuranceStatus InsuranceStatus { get; init; }
}