using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeDailyWorkingHourHeader
{
    public int WorkingHourHeaderId { get; set; }

    public int EmployeeMasterId { get; set; }

    public int ShiftId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public virtual ICollection<EmployeeDailyWorkingHourDetail> EmployeeDailyWorkingHourDetails { get; set; } = new List<EmployeeDailyWorkingHourDetail>();

    public virtual EmployeeMaster EmployeeMaster { get; set; } = null!;

    public virtual ShiftMaster Shift { get; set; } = null!;
}
