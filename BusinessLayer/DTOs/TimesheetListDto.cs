using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class TimesheetListDto
    {
        public int TimesheetId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public DateTime TimesheetDate { get; set; }
        public string Status { get; set; } = null!;
        public List<TimesheetProjectDto> Projects { get; set; } = new();
    }
}
