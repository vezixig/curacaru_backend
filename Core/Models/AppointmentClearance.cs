namespace Curacaru.Backend.Core.Models;

using Entities;
using Enums;

public class AppointmentClearance
{
    public ClearanceType ClearanceType { get; set; }

    public Customer Customer { get; set; }

    public Employee Employee { get; set; }

    public List<Employee> ReplacementEmployee { get; set; } = [];
}