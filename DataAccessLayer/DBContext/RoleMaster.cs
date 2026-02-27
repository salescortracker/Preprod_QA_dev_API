using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class RoleMaster
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CompanyId { get; set; }

    public int? RegionId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();

    public virtual ICollection<MenuRoleMaster> MenuRoleMasters { get; set; } = new List<MenuRoleMaster>();
}
