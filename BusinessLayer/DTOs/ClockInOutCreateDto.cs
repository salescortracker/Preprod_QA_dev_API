using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ClockInOutCreateDto
    {
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public int Department { get; set; } 
        public DateTime AttendanceDate { get; set; }
        public string ActionType { get; set; } = null!; // "ClockIn" / "ClockOut"
        public string ActionTime { get; set; } = null!;   // "HH:mm"
        public string clockInTime { get; set; }
        public string clockOutTime { get; set; }
    }
}

