namespace Curacaru.Backend.Core.Serializer;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Exceptions;

/// <summary>Serializes <see cref="TimeOnly" /> to match the angular time type.</summary>
internal class TimeOnlySerializer : JsonConverter<TimeOnly>
{
    /// <inheritdoc />
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var json = JsonDocument.ParseValue(ref reader);
        var value = json.RootElement.GetString()
                    ?? throw new BadRequestException("could not read value");
        return TimeOnly.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue($"{value:HH:mm}");
}