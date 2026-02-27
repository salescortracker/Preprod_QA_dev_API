using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ShiftAllocation
{
    public int ShiftAllocationId { get; set; }

    public int UserId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string ShiftName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ShiftId { get; set; }

    public virtual ShiftMaster? Shift { get; set; }
}
