using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class AssetMaster
{
    public int AssetId { get; set; }

    public string? AssetName { get; set; }

    public string? AssetCode { get; set; }

    public string? AssetLocation { get; set; }

    public decimal? AssetCost { get; set; }

    public string? AssetDescription { get; set; }

    public string? AssetModelOrVersion { get; set; }

    public string? PurchaseOrder { get; set; }

    public DateOnly? WarrantyStartDate { get; set; }

    public DateOnly? WarrantyEndDate { get; set; }

    public DateOnly? AssetReturnDate { get; set; }

    public string? Status { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CompanyId { get; set; }

    public int? RegionId { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }
}
