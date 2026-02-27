using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MissedPunchRequestDto
    {
        public int MissedPunchRequestID { get; set; }
        public int EmployeeID { get; set; }
        public int ManagerID { get; set; }
        public DateTime MissedDate { get; set; }
        public int? MissedTypeID { get; set; }
        public string? MissedType { get; set; }
        public DateTime? CorrectClockIn { get; set; }
        public DateTime? CorrectClockOut { get; set; }
        public string Reason { get; set; } = null!;
        public string? ManagerRemarks { get; set; }
        public string Status { get; set; } = "Pending";
        public int CompanyID { get; set; }
        public int? RegionID { get; set; }
    }
    public class MissedPunchActionDto
    {
        public int RequestId { get; set; }
        public int ManagerId { get; set; }
        public string Status { get; set; } = null!; // "Approved" or "Rejected"
        public string? ManagerRemarks { get; set; }
    }
}
