using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class AttendanceConfiguration
{
    public int AttendanceConfigurationId { get; set; }

    public string Mode { get; set; } = null!;

    public int GracePeriod { get; set; }

    public int LateMarkThreshold { get; set; }

    public int EarlyLeaveThreshold { get; set; }

    public bool ShiftRequired { get; set; }

    public bool AutoAbsent { get; set; }

    public string? OvertimeCalculation { get; set; }

    public string? Description { get; set; }

    public TimeOnly? ShiftStartTime { get; set; }

    public TimeOnly? ShiftEndTime { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }
}
