using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CountryMaster
{
    public int CountryId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string CountryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
