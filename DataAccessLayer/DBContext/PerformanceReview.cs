using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class PerformanceReview
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public string? DepartmentProject { get; set; }

    public int? ReportingManagerId { get; set; }

    public string? Designation { get; set; }

    public string? Department { get; set; }

    public DateOnly? DateOfJoining { get; set; }

    public string? ProbationStatus { get; set; }

    public string? PerformanceCycle { get; set; }

    public DateOnly? ApplicableStartDate { get; set; }

    public DateOnly? ApplicableEndDate { get; set; }

    public string? ProgressType { get; set; }

    public string? AppraisalYear { get; set; }

    public string? DocumentEvidence { get; set; }

    public string? SelfReviewSummary { get; set; }

    public string? Status { get; set; }

    public decimal? FinalScore { get; set; }

    public string? ManagerRemarks { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedBy { get; set; }
    public virtual ICollection<PerformanceKpi> KPIs { get; set; }
}
