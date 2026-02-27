using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class VisaTypeMaster
{
    public int VisaTypeId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string VisaTypeName { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
