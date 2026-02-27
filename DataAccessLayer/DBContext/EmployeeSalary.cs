using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeSalary
{
    public int EmployeeSalaryId { get; set; }

    public int EmployeeId { get; set; }

    public int StructureId { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public decimal Ctc { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public string? CompanyId { get; set; }

    public string? RegionId { get; set; }

    public virtual SalaryStructure Structure { get; set; } = null!;
}
