using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Asset
{
    public int AssetId { get; set; }

    public string AssetName { get; set; } = null!;

    public string AssetCode { get; set; } = null!;

    public string? AssetLocation { get; set; }

    public decimal AssetCost { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string? AssetDescription { get; set; }

    public string? AssetModel { get; set; }

    public string? PurchaseOrder { get; set; }

    public DateOnly? WarrantyStartDate { get; set; }

    public DateOnly? WarrantyEndDate { get; set; }

    public DateOnly? AssetReturnDate { get; set; }

    public int AssetStatusId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int UserId { get; set; }

    public string? EmployeeName { get; set; }

    public int? ReportingTo { get; set; }

    public string ApprovalStatus { get; set; } = null!;

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public virtual AssetStatus AssetStatus { get; set; } = null!;
}
