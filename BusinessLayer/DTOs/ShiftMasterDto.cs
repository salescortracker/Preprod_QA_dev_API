using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ShiftMasterDto
    {
        public int ShiftID { get; set; }
        public string? ShiftName { get; set; }
        public string? ShiftStartTime { get; set; }
        public string? ShiftEndTime { get; set; }
        public int? GraceTime { get; set; }
        public bool IsActive { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int? UserId { get; set; }
        public string? CompanyName { get; set; }
        public string? RegionName { get; set; }
    }
}
