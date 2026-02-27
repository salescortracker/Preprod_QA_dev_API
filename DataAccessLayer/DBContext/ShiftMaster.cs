using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ShiftMaster
{
    public int ShiftId { get; set; }

    public string ShiftName { get; set; } = null!;

    public TimeOnly ShiftStartTime { get; set; }

    public TimeOnly ShiftEndTime { get; set; }

    public int? GraceTime { get; set; }

    public bool IsActive { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<EmployeeDailyWorkingHourHeader> EmployeeDailyWorkingHourHeaders { get; set; } = new List<EmployeeDailyWorkingHourHeader>();

    public virtual ICollection<ShiftAllocation> ShiftAllocations { get; set; } = new List<ShiftAllocation>();
}
