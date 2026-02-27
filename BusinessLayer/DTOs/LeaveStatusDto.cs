using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class LeaveStatusDto
    {
        public int LeaveStatusID { get; set; }
        public string LeaveStatusName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int ModifiedBy { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public int CreatedBy { get; set; }
        public int UserID { get; set; }
    }
}
