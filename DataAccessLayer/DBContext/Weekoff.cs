using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Weekoff
{
    public int WeekoffId { get; set; }

    public int? UserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public DateOnly Weekoff1 { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
