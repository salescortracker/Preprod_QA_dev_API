using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeKpi
{
    public int Kpiid { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string EmployeeNameId { get; set; } = null!;

    public int ReportingManagerId { get; set; }

    public string? Designation { get; set; }

    public string? DepartmentId { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public string? ProbationStatus { get; set; }

    public string? PerformanceCycle { get; set; }

    public DateOnly? ApplicableStartDate { get; set; }

    public DateOnly? ApplicableEndDate { get; set; }

    public string? ProgressType { get; set; }

    public int? AppraisalYear { get; set; }

    public string? DocumentEvidencePath { get; set; }

    public string? SelfReviewSummary { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<EmployeeKpiitem> EmployeeKpiitems { get; set; } = new List<EmployeeKpiitem>();
}
