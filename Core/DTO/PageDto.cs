namespace Curacaru.Backend.Core.DTO;

/// <summary>A DTO for a page.</summary>
public record PageDto<T>(List<T> Items, int Page, int PageCount)
{
}