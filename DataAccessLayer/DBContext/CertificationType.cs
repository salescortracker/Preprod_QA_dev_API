using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CertificationType
{
    public int CertificationTypeId { get; set; }

    public string CertificationTypeName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public bool IsDeleted { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<EmployeeCertification> EmployeeCertifications { get; set; } = new List<EmployeeCertification>();
}
