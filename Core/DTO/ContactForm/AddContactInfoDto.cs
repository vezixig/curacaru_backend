namespace Curacaru.Backend.Core.DTO.ContactForm;

using Enums;

public record AddContactInfoDto
{
    public int CareLevel { get; set; }

    public string? Contact { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public List<int> Products { get; set; }

    public Gender Salutation { get; set; }

    public string Street { get; set; }

    public string ZipCode { get; set; }
}