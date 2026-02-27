using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ModeOfStudy
{
    public int ModeOfStudyId { get; set; }

    public string ModeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; } = new List<EmployeeEducation>();
}
