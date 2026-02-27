using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ExpenseCategory
{
    public int ExpenseCategoryId { get; set; }

    public string ExpenseCategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int? SortOrder { get; set; }

    public string? Description { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ExpenseLimitConfig> ExpenseLimitConfigs { get; set; } = new List<ExpenseLimitConfig>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
