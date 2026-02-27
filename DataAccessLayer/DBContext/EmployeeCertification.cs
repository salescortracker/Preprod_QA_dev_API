using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeCertification
{
    public int CertificationId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string CertificationName { get; set; } = null!;

    public int CertificationTypeId { get; set; }

    public string? Description { get; set; }

    public string? DocumentPath { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsActive { get; set; }

    public int UserId { get; set; }

    public virtual CertificationType CertificationType { get; set; } = null!;
}
