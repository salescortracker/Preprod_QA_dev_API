using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class MenuMaster
{
    public int MenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public int? ParentMenuId { get; set; }

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int? OrderNo { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<MenuRoleMaster> MenuRoleMasters { get; set; } = new List<MenuRoleMaster>();
}
