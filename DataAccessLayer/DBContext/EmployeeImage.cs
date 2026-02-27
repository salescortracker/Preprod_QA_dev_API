using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeImage
{
    public int Id { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
