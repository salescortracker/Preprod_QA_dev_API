using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class PerformanceKpi
{
    public int Id { get; set; }

    public int? PerformanceReviewId { get; set; }

    public string? Kpiname { get; set; }

    public decimal? Weightage { get; set; }

    public string? Target { get; set; }

    public string? Achieved { get; set; }

    public int? SelfRating { get; set; }

    public int? ManagerRating { get; set; }

    public string? Remarks { get; set; }

    public decimal? Score { get; set; }
}
