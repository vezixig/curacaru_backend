namespace Curacaru.Backend.Application.Services;

public interface IDateTimeService
{
    /// <summary>Gets the current date and time.</summary>
    DateTime Now { get; }

    /// <summary>Gets the current date.</summary>
    DateOnly Today { get; }
}