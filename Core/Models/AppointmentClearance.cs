namespace Curacaru.Backend.Core.Models;

using Entities;
using Enums;

public class AppointmentClearance
{
    public ClearanceType ClearanceType { get; set; }

    public required Customer Customer { get; set; }

    public List<Employee> Employees { get; set; } = [];

    public List<Employee> ReplacementEmployee { get; set; } = [];
}