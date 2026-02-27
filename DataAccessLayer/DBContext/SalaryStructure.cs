using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class SalaryStructure
{
    public int StructureId { get; set; }

    public string StructureName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public int? DesignationId { get; set; }

    public int? GradeId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public string? CompanyId { get; set; }

    public string? RegionId { get; set; }

    public virtual ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();

    public virtual ICollection<SalaryStructureComponent> SalaryStructureComponents { get; set; } = new List<SalaryStructureComponent>();
}
