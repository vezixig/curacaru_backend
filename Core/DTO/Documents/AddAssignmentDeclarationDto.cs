namespace Curacaru.Backend.Core.DTO.AssignmentDeclaration;

public class AddAssignmentDeclarationDto
{
    public Guid CustomerId { get; set; }

    public string Signature { get; set; } = "";

    public string SignatureCity { get; set; } = "";

    public int Year { get; set; }
}