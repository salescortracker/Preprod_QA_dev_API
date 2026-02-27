using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmploymentTypeDto
    {
        public int EmploymenttypeID { get; set; }
        public int? UserId { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string EmploymenttypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
