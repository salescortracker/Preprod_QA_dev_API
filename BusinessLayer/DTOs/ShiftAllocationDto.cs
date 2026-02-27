using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ShiftAllocationDto
    {
        public int ShiftAllocationId { get; set; }
        public int UserID { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        // Foreign Key to ShiftMaster
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
