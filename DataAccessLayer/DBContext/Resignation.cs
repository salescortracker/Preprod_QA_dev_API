using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Resignation
{
    public int ResignationId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string ResignationType { get; set; } = null!;

    public int NoticePeriodDays { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? UserId { get; set; }
}
