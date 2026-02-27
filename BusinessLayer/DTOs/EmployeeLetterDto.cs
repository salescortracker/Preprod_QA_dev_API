using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeLetterDto
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public DateOnly IssuedDate { get; set; }
        public DateOnly ValidityDate { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? Remarks { get; set; }
        public bool IsConfidential { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // File Upload
        public IFormFile? DocumentFile { get; set; }
    }
}
