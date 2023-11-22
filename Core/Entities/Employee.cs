﻿namespace Curacaru.Backend.Core.Entities;

using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Required]
    public string AuthId { get; set; }

    public Guid? CompanyId { get; set; }

    public string Email { get; set; } = "";

    public string FirstName { get; set; } = "";

    [Key]
    public Guid Id { get; set; }

    public string LastName { get; set; } = "";
}