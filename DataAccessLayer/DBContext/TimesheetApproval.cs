using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class TimesheetApproval
{
    public int ApprovalId { get; set; }

    public int TimesheetId { get; set; }

    public int ApproverUserId { get; set; }

    public string Status { get; set; } = null!;

    public string? ApproverComments { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual User ApproverUser { get; set; } = null!;

    public virtual Timesheet Timesheet { get; set; } = null!;
}
