using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class SalaryStructureComponent
{
    public int Id { get; set; }

    public int StructureId { get; set; }

    public int ComponentId { get; set; }

    public decimal Value { get; set; }

    public string CalculationType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public int? UserId { get; set; }

    public string? CompanyId { get; set; }

    public string? RegionId { get; set; }

    public virtual SalaryComponent Component { get; set; } = null!;

    public virtual SalaryStructure Structure { get; set; } = null!;
}
