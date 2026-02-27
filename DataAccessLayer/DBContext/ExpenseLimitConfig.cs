using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ExpenseLimitConfig
{
    public int ExpenseLimitConfigId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int ExpenseCategoryId { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public decimal PerTransactionLimit { get; set; }

    public decimal? DailyLimit { get; set; }

    public decimal? MonthlyLimit { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
}
