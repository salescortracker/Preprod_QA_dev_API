using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class MissedType
{
    public int MissedTypeId { get; set; }

    public int CompanyId { get; set; }

    public int? RegionId { get; set; }

    public string MissedType1 { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
