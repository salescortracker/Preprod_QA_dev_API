using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeJobHistory
{
    public int Id { get; set; }

    public string Employer { get; set; } = null!;

    public string? UploadDocument { get; set; }

    public string JobTitle { get; set; } = null!;

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public decimal LastCtc { get; set; }

    public string Website { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string ReasonForLeaving { get; set; } = null!;

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int UserId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }
}
