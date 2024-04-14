namespace Curacaru.Backend.Core.DTO.TimeTracker;

using System.Text.Json.Serialization;
using Serializer;

public class GetWorkedHoursDto
{
    public Guid AppointmentId { get; set; }

    public DateOnly Date { get; set; }

    public bool IsDone { get; set; }

    public bool IsPlanned { get; set; }

    [JsonConverter(typeof(TimeOnlySerializer))]
    public TimeOnly TimeEnd { get; set; }

    [JsonConverter(typeof(TimeOnlySerializer))]
    public TimeOnly TimeStart { get; set; }

    public double WorkDuration { get; set; }
}