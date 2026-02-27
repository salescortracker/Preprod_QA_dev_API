using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeEducation
{
    public int EducationId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int? ModeOfStudyId { get; set; }

    public int UserId { get; set; }

    public string Qualification { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string Institution { get; set; } = null!;

    public string? Board { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Result { get; set; } = null!;

    public string? CertificateFilePath { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ModeOfStudy? ModeOfStudy { get; set; }
}
