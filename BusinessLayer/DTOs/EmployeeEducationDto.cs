using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeEducationDto
    {
        public int EducationId { get; set; }

        public int UserId { get; set; }       // ← Replaced EmployeeId
        public int CompanyId { get; set; }
        public int RegionId { get; set; }

        public int? ModeOfStudyId { get; set; }

        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public string Institution { get; set; }
        public string Board { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public string? Result { get; set; }
        public string? CertificateFilePath { get; set; }

        public IFormFile? CertificateFile { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
