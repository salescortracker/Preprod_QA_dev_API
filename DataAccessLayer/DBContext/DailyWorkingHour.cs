using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class DailyWorkingHour
{
    public int DailyWorkingHourId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int UserId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public int ShiftId { get; set; }

    public byte DayOfWeek { get; set; }

    public bool IsWorking { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public decimal? TotalHours { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ShiftMaster Shift { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
