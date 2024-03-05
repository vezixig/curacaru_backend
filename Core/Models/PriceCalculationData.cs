namespace Curacaru.Backend.Core.Models;

using DTO.Appointment;
using Entities;

public class PriceCalculationData
{
    public Guid CompanyId { get; set; }

    public decimal DistanceToCustomer { get; set; }

    public TimeOnly TimeEnd { get; set; }

    public TimeOnly TimeStart { get; set; }

    /// <summary>
    ///     Creates a new instance of the <see cref="PriceCalculationData" /> class from the given
    ///     <see cref="Appointment" />.
    /// </summary>
    public static PriceCalculationData CreateFrom(Appointment appointment)
        => new()
        {
            CompanyId = appointment.CompanyId,
            DistanceToCustomer = appointment.DistanceToCustomer,
            TimeEnd = appointment.TimeEnd,
            TimeStart = appointment.TimeStart
        };

    /// <summary>
    ///     Creates a new instance of the <see cref="PriceCalculationData" /> class from the given
    ///     <see cref="UpdateAppointmentDto" />.
    /// </summary>
    public static PriceCalculationData CreateFrom(UpdateAppointmentDto appointment, Guid companyId)
        => new()
        {
            CompanyId = companyId,
            DistanceToCustomer = appointment.DistanceToCustomer,
            TimeEnd = appointment.TimeEnd,
            TimeStart = appointment.TimeStart
        };
}