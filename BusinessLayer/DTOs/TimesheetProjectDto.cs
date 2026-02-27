using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class TimesheetProjectDto
    {
        public string ProjectName { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public string TotalHoursText { get; set; } = null!;

        public int TotalMinutes { get; set; }   // required
        public int? OTMinutes { get; set; }     // optional
        public string? OTHoursText { get; set; }
    }
}
