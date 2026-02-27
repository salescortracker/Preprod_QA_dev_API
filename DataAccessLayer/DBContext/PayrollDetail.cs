using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class PayrollDetail
{
    public int PayrollDetailId { get; set; }

    public int PayrollId { get; set; }

    public int ComponentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public string? CompanyId { get; set; }

    public string? RegionId { get; set; }

    public virtual SalaryComponent Component { get; set; } = null!;

    public virtual PayrollTransaction Payroll { get; set; } = null!;
}
