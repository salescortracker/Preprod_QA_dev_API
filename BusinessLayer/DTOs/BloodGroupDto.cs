using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class BloodGroupDto
    {
        public int BloodGroupID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string BloodGroupName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
      
        public int UserID { get; set; }
    }
    public class BloodGroupResponseDto
    {
        public int BloodGroupID { get; set; }
        public string BloodGroupName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
