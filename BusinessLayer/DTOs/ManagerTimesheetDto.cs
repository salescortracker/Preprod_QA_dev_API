using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ManagerTimesheetDto
    {
        public int TimesheetId { get; set; }
        public int UserId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime TimesheetDate { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public int OtMinutes { get; set; }
        public string OtHoursText { get; set; }
        public List<TimesheetProjectDto> Projects { get; set; }
    }
}
