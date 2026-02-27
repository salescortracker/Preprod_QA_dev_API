using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class LeaveRequest
{
    public int LeaveRequestId { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int LeaveTypeId { get; set; }

    public bool? IsHalfDay { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal TotalDays { get; set; }

    public string Reason { get; set; } = null!;

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public int? ReportingManagerId { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedDate { get; set; }

    public int? ApprovedRejectedBy { get; set; }

    public DateTime? ApprovedRejectedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual LeaveType LeaveType { get; set; } = null!;

    public virtual User? ReportingManager { get; set; }

    public virtual User User { get; set; } = null!;
}
