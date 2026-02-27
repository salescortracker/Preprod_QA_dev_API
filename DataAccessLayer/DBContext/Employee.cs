using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? FullName { get; set; }

    public bool? IsActive { get; set; }
}
