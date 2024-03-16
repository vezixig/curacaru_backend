namespace Curacaru.Backend.Core.DTO.TimeTracker;

public class AddWorkingTimeReportSignatureDto
{
    public Guid EmployeeId { get; set; }

    public int Month { get; set; }

    public string Signature { get; set; } = "";

    public string SignatureCity { get; set; } = "";

    public DateOnly SignatureDate { get; set; }

    public int Year { get; set; }
}