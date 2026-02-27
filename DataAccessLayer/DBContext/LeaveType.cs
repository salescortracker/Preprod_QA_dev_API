using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string LeaveTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int LeaveDays { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual Region Region { get; set; } = null!;
}
