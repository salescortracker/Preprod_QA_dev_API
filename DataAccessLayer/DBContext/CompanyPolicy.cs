using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CompanyPolicy
{
    public int PolicyId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string Title { get; set; } = null!;

    public int CategoryId { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public string? Description { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual PolicyCategory Category { get; set; } = null!;
}
