namespace Curacaru.Backend.Core.DTO.TimeTracker;

using System.Text.Json.Serialization;
using Serializer;

public class GetWorkingHoursDto
{
    public DateOnly Date { get; set; }

    [JsonConverter(typeof(TimeOnlySerializer))]
    public TimeOnly TimeEnd { get; set; }

    [JsonConverter(typeof(TimeOnlySerializer))]
    public TimeOnly TimeStart { get; set; }

    public double WorkDuration { get; set; }
}