using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeDdlist
{
    public int DdlistId { get; set; }

    public int EmployeeId { get; set; }

    public int? RegionId { get; set; }

    public int? UserId { get; set; }

    public int? CompanyId { get; set; }

    public string Ddnumber { get; set; } = null!;

    public DateOnly Dddate { get; set; }

    public string BankName { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string PayeeName { get; set; } = null!;

    public string? DdcopyFilePath { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }
}
