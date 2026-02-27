using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeDailyWorkingHourDetail
{
    public int WorkingHourDetailId { get; set; }

    public int WorkingHourHeaderId { get; set; }

    public int DayOfWeek { get; set; }

    public bool? IsWorkingDay { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public int? TotalMinutes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public virtual EmployeeDailyWorkingHourHeader WorkingHourHeader { get; set; } = null!;
}
