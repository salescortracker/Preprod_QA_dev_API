using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MenuMasterDto
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public int? ParentMenuID { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? OrderNo { get; set; }
        public bool? IsActive { get; set; }
    }
    public class CreateMenuMasterDto
    {
        public string MenuName { get; set; } = string.Empty;
        public int? ParentMenuID { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? OrderNo { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateMenuMasterDto : CreateMenuMasterDto
    {
        public int MenuID { get; set; }
        public int ModifiedBy { get; set; }
    }
}
