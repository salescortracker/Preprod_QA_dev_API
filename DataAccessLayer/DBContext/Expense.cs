using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public string? ProjectName { get; set; }

    public string? Location { get; set; }

    public string? Country { get; set; }

    public int? ExpenseCategoryId { get; set; }

    public int? DepartmentId { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly? ExpenseDate { get; set; }

    public string? Reason { get; set; }

    public string Status { get; set; } = null!;

    public string? ReferenceNo { get; set; }

    public bool HasReceipt { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? ReceiptPath { get; set; }

    public string? DepartmentName { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ExpenseCategory? ExpenseCategory { get; set; }
}
