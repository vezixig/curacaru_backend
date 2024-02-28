namespace Curacaru.Backend.Application.Services.Implementations;

internal class DateTimeService : IDateTimeService
{
    public DateTime Now { get; } = DateTime.Now;

    public DateOnly Today { get; } = DateOnly.FromDateTime(DateTime.Today);
}