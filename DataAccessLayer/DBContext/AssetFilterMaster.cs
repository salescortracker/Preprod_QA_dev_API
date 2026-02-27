using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class AssetFilterMaster
{
    public int Id { get; set; }

    public string? AssetName { get; set; }

    public string? Location { get; set; }

    public string? Status { get; set; }

    public string? Currency { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? CompanyId { get; set; }

    public int? RegionId { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }
}
