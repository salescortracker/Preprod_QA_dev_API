using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class KpiCategory
{
    public int KpiCategoryId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string KpiCategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int UserId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
