using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class SalaryComponent
{
    public int ComponentId { get; set; }

    public string ComponentName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string CalculationType { get; set; } = null!;

    public string? PercentageOf { get; set; }

    public bool IsTaxable { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public string? CompanyId { get; set; }

    public string? RegionId { get; set; }

    public virtual ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();

    public virtual ICollection<SalaryStructureComponent> SalaryStructureComponents { get; set; } = new List<SalaryStructureComponent>();
}
