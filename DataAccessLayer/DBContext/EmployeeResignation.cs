using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeResignation
{
    public int ResignationId { get; set; }

    public string? EmployeeId { get; set; }

    public string? ResignationType { get; set; }

    public string? NoticePeriod { get; set; }

    public DateOnly? LastWorkingDay { get; set; }

    public string? ResignationReason { get; set; }

    public string? Status { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CompanyId { get; set; }

    public int? RegionId { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public DateTime? ManagerApprovedDate { get; set; }

    public DateTime? ManagerRejectedDate { get; set; }

    public DateTime? HrApprovedDate { get; set; }

    public DateTime? HrRejectedDate { get; set; }

    public string? ManagerReason { get; set; }

    public string? HrReason { get; set; }
}
