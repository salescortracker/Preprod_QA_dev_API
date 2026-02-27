using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class WfhremoteRequest
{
    public int WfhrequestId { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string RequestType { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? DocumentPath { get; set; }

    public string Status { get; set; } = null!;

    public int? ManagerId { get; set; }

    public string? ManagerRemarks { get; set; }

    public int CompanyId { get; set; }

    public int? RegionId { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ApprovedOn { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
