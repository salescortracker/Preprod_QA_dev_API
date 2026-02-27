using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeReference
{
    public int ReferenceId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string TitleOrDesignation { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public int UserId { get; set; }
}
