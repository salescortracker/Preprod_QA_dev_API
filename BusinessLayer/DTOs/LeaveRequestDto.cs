using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class LeaveRequestDto
    {
        public int LeaveRequestId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public int LeaveTypeId { get; set; }
        public bool IsHalfDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; }
        public int? ReportingManagerId { get; set; }
        public string? EmployeeName { get; set; }   // ✅ Add this


        // File handling
        public IFormFile? SupportingDocument { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        // Listing fields
        public string? LeaveTypeName { get; set; }
        public string? Status { get; set; }
        public DateTime? AppliedDate { get; set; }
    }
}
