namespace Curacaru.Backend.Application.Services.Implementations;

internal class DateTimeService : IDateTimeService
{
    public DateOnly BeginOfCurrentMonth => new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public DateOnly EndOfMonth => new DateOnly(Today.Year, Today.Month, 1).AddMonths(1).AddDays(-1);

    public DateTime Now => DateTime.Now;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}