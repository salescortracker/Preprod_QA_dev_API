using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class LeaveListDto
    {
        public int LeaveRequestId { get; set; }
        public string LeaveType { get; set; }
        public string AppliedDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; }
        public string? FileName { get; set; }
        public string Status { get; set; }
    }
}
