using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeW4
{
    public int W4Id { get; set; }

    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleInitial { get; set; }

    public string LastName { get; set; } = null!;

    public string Ssn { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string FilingStatus { get; set; } = null!;

    public bool? MultipleJobsOrSpouse { get; set; }

    public int? TotalDependents { get; set; }

    public decimal? DependentAmounts { get; set; }

    public decimal? OtherIncome { get; set; }

    public decimal? Deductions { get; set; }

    public decimal? ExtraWithholding { get; set; }

    public string EmployeeSignature { get; set; } = null!;

    public DateOnly FormDate { get; set; }

    public int? RegionId { get; set; }

    public int? UserId { get; set; }

    public int? CompanyId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }
}
