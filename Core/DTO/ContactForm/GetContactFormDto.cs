namespace Curacaru.Backend.Core.DTO.ContactForm;

public record GetContactFormDto(
    string Color,
    int FontSize,
    Guid? Id,
    bool IsRounded)
{
}