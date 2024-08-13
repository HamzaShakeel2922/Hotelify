using System;
using System.Collections.Generic;

namespace ReactApp1.Server.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public int? Salary { get; set; }

    public string? Department { get; set; }

    public string? Position { get; set; }

    public DateOnly? HireDate { get; set; }
}
