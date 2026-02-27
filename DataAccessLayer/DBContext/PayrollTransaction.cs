using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class PayrollTransaction
{
    public int PayrollId { get; set; }

    public int EmployeeId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal NetSalary { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public string? CompanyId { get; set; }

    public string? RegionId { get; set; }

    public virtual ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();
}
