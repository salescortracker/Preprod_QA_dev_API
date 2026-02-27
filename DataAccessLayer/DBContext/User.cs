using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class User
{
    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string? EmployeeCode { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public int? ReportingTo { get; set; }

    public int? DepartmentId { get; set; }

    public string? CompanyName { get; set; }

    public string? Type { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Module { get; set; }

    public bool? Userloginstatus { get; set; }

    public bool? Passwordchanged { get; set; }

    public int? UserCompanyId { get; set; }

    public string? Designation { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<EmployeeMaster> EmployeeMasterCreatedByNavigations { get; set; } = new List<EmployeeMaster>();

    public virtual ICollection<EmployeeMaster> EmployeeMasterManagers { get; set; } = new List<EmployeeMaster>();

    public virtual ICollection<EmployeeMaster> EmployeeMasterUpdatedByNavigations { get; set; } = new List<EmployeeMaster>();

    public virtual ICollection<LeaveRequest> LeaveRequestReportingManagers { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<LeaveRequest> LeaveRequestUsers { get; set; } = new List<LeaveRequest>();

    public virtual Region Region { get; set; } = null!;

    public virtual ICollection<TimesheetApproval> TimesheetApprovals { get; set; } = new List<TimesheetApproval>();

    public virtual ICollection<Timesheet> TimesheetManagerUsers { get; set; } = new List<Timesheet>();

    public virtual ICollection<Timesheet> TimesheetUsers { get; set; } = new List<Timesheet>();
}
