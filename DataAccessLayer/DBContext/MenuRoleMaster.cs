using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class MenuRoleMaster
{
    public int MenuRoleId { get; set; }

    public int RoleId { get; set; }

    public int MenuId { get; set; }

    public bool? CanView { get; set; }

    public bool? CanAdd { get; set; }

    public bool? CanEdit { get; set; }

    public bool? CanDelete { get; set; }

    public bool? CanApprove { get; set; }

    public bool? IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual MenuMaster Menu { get; set; } = null!;

    public virtual RoleMaster Role { get; set; } = null!;
}
