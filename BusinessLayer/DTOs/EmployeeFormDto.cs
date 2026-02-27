using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeFormDto
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public DateOnly IssueDate { get; set; }
        [JsonIgnore]
        public string? FileName { get; set; }

        public string? FilePath { get; set; }
        public string? Remarks { get; set; }
        public bool IsConfidential { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // File Upload (Like Education)
        public IFormFile? UploadFile { get; set; }
    }
}
