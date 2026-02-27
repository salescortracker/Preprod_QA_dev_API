using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeJobHistoryDto
    {
        public int Id { get; set; }
        public string Employer { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public DateTime FromDate { get; set; }      // Use DateTime
        public DateTime ToDate { get; set; }        // Use DateTime
        public decimal LastCTC { get; set; }
        public string Website { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public string ReasonForLeaving { get; set; } = null!;
        public string? UploadDocumentPath { get; set; }
        public IFormFile? UploadDocument { get; set; }

        public int CompanyId { get; set; }   // renamed
        public int RegionId { get; set; }    // renamed
        public int UserId { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
