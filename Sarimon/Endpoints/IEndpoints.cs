namespace Curacaru.Backend.Endpoints;

/// <summary>Used for endpoints.</summary>
public interface IEndpoints
{
    /// <summary>Maps the endpoints.</summary>
    /// <param name="app">The application to map the endpoints to.</param>
    void MapEndpoints(WebApplication app);
}