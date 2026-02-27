using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class MissedPunchRequest
{
    public int MissedPunchRequestId { get; set; }

    public int EmployeeId { get; set; }

    public int? ManagerId { get; set; }

    public DateOnly MissedDate { get; set; }

    public string? MissedType { get; set; }

    public TimeOnly? CorrectClockIn { get; set; }

    public TimeOnly? CorrectClockOut { get; set; }

    public string Reason { get; set; } = null!;

    public string? ManagerRemarks { get; set; }

    public string Status { get; set; } = null!;

    public int CompanyId { get; set; }

    public int? RegionId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public int UserId { get; set; }
}
