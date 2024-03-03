namespace Curacaru.Backend.Application.Services.Implementations;

internal class DateTimeService : IDateTimeService
{
    public DateOnly EndOfMonth => new DateOnly(Today.Year, Today.Month, 1).AddMonths(1).AddDays(-1);

    public DateTime Now { get; } = DateTime.Now;

    public DateOnly Today { get; } = DateOnly.FromDateTime(DateTime.Today);
}