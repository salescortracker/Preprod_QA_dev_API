using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MenuRoleDto
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
    }

    public class RolePermissionRequestDto
    {
        public int RoleId { get; set; }
        public List<MenuRoleDto> Permissions { get; set; } = new();
    }

}
