using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MenuRoleMasterDto
    {
        public int MenuRoleId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string icon { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string? MenuUrl { get; set; }
        public int? ParentId { get; set; }
        public int? OrderNo { get; set; }

        public bool? CanView { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? IsActive { get; set; }
    }
}
