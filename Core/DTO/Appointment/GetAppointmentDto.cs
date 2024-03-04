﻿namespace Curacaru.Backend.Core.DTO.Appointment;

using Customer;
using Entities;
using Enums;

public class GetAppointmentDto
{
    /// <inheritdoc cref="Appointment.ClearanceType" />
    public ClearanceType ClearanceType { get; set; }

    /// <inheritdoc cref="Appointment.Costs" />
    public decimal Costs { get; set; }

    /// <inheritdoc cref="Appointment.CostsLastYearBudget" />
    public decimal CostsLastYearBudget { get; set; }

    /// <inheritdoc cref="Appointment.Customer" />
    public GetMinimalCustomerListEntryDto Customer { get; set; } = new();

    /// <inheritdoc cref="Appointment.CustomerId" />
    public Guid CustomerId { get; set; }

    /// <inheritdoc cref="Appointment.Date" />
    public DateOnly Date { get; set; }

    /// <inheritdoc cref="Appointment.DistanceToCustomer" />
    public int DistanceToCustomer { get; set; }

    /// <inheritdoc cref="Appointment.EmployeeId" />
    public Guid EmployeeId { get; set; }

    /// <inheritdoc cref="Appointment.EmployeeReplacementId" />
    public Guid? EmployeeReplacementId { get; set; }

    /// <inheritdoc cref="Appointment.HasBudgetError" />
    public bool HasBudgetError { get; set; }

    /// <inheritdoc cref="Appointment.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="Appointment.IsDone" />
    public bool IsDone { get; set; }

    /// <inheritdoc cref="Appointment.IsPlanned" />
    public bool IsPlanned { get; set; }

    /// <inheritdoc cref="Appointment.IsSignedByCustomer" />
    public bool IsSignedByCustomer { get; set; }

    /// <inheritdoc cref="Appointment.IsSignedByEmployee" />
    public bool IsSignedByEmployee { get; set; }

    /// <inheritdoc cref="Appointment.Notes" />
    public string Notes { get; set; } = "";

    /// <inheritdoc cref="Appointment.TimeEnd" />
    public TimeOnly TimeEnd { get; set; }

    /// <inheritdoc cref="Appointment.TimeStart" />
    public TimeOnly TimeStart { get; set; }
}