using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Asset1
{
    public int AssetId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int? UserId { get; set; }

    public string AssetName { get; set; } = null!;

    public string AssetCode { get; set; } = null!;

    public string? AssetLocation { get; set; }

    public string? AssetCost { get; set; }

    public string? AssetDesc { get; set; }

    public string? AssetModel { get; set; }

    public string? PurchaseOrder { get; set; }

    public DateTime? WarrantyStartDate { get; set; }

    public DateTime? WarrantyEndDate { get; set; }

    public DateTime? AssetReturnDate { get; set; }

    public int AssetStatus { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
