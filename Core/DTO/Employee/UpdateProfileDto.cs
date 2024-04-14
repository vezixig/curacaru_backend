namespace Curacaru.Backend.Core.DTO.Employee;

public record UpdateProfileDto(
    string FirstName,
    string LastName,
    string PhoneNumber)
{
}