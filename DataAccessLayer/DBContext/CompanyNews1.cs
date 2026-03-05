using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CompanyNews1
{
    public int NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string? AttachmentPath { get; set; }

    public string? AttachmentName { get; set; }

    public bool IsActive { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UserId { get; set; }
}
