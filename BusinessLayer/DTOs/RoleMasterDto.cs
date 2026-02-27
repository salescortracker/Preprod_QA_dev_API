using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class RoleMasterDto
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public bool IsActive { get; set; }
        public string? RoleDescription { get; set; }
     
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public int? CompanyId { get; set; }
        public int? RegionId { get; set; }
        public int UserId { get; set; }
        public string? CompanyName { get; set; }
        public string? RegionName { get; set; }
    }
}
