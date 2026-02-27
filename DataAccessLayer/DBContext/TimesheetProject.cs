using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class TimesheetProject
{
    public int ProjectId { get; set; }

    public int TimesheetId { get; set; }

    public string ProjectName { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int TotalMinutes { get; set; }

    public string TotalHoursText { get; set; } = null!;

    public int? Otminutes { get; set; }

    public string? OthoursText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Timesheet Timesheet { get; set; } = null!;
}
