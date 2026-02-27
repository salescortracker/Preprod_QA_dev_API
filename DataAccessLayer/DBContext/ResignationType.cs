using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ResignationType
{
    public int ResignationTypeId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string ResignationTypeName { get; set; } = null!;

    public int NoticePeriod { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
