using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Department
{
    public int DepartmentId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? UserId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Designation> Designations { get; set; } = new List<Designation>();

    public virtual ICollection<ExpenseLimitConfig> ExpenseLimitConfigs { get; set; } = new List<ExpenseLimitConfig>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual Region Region { get; set; } = null!;
}
