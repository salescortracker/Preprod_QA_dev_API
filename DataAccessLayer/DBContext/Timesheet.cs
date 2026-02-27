using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Timesheet
{
    public int TimesheetId { get; set; }

    public int UserId { get; set; }

    public int? ManagerUserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public DateOnly TimesheetDate { get; set; }

    public string? Comments { get; set; }

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public string Status { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual User? ManagerUser { get; set; }

    public virtual ICollection<TimesheetApproval> TimesheetApprovals { get; set; } = new List<TimesheetApproval>();

    public virtual ICollection<TimesheetProject> TimesheetProjects { get; set; } = new List<TimesheetProject>();

    public virtual User User { get; set; } = null!;
}
