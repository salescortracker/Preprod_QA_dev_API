using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class TimesheetRequestDto
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }

        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public DateTime TimesheetDate { get; set; }

        public string? Comments { get; set; }

        public IFormFile? Attachment { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        public List<TimesheetProjectDto> Projects { get; set; } = new();
    }
}
