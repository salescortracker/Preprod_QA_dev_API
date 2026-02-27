using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeKpiitem
{
    public int KpiitemId { get; set; }

    public int Kpiid { get; set; }

    public string? Kpiobjective { get; set; }

    public int? Weightage { get; set; }

    public string? Target { get; set; }

    public string? TaskCompleted { get; set; }

    public int? SelfRating { get; set; }

    public string? Remarks { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual EmployeeKpi Kpi { get; set; } = null!;
}
