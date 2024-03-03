namespace Curacaru.Backend.Application.Services;

public interface IDateTimeService
{
    /// <summary>Gets the end of the current month.</summary>
    DateOnly EndOfMonth { get; }

    /// <summary>Gets the current date and time.</summary>
    DateTime Now { get; }

    /// <summary>Gets the current date.</summary>
    DateOnly Today { get; }
}