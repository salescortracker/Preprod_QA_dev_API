using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ClockInOutDto
    {
        public int ClockInOutId { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public int Department { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? ClockInTime { get; set; }
        public TimeSpan? ClockOutTime { get; set; }
        public int? TotalMinutes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ActionType { get; set; }
        public TimeOnly? ActionTime { get; set; }
    }
}
