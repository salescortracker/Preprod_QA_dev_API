using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeCertificationDto
    {
        public int CertificationId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public int UserId { get; set; }

        public string? CertificationName { get; set; }
        public int CertificationTypeId { get; set; }
        public string? Description { get; set; }

        public string? DocumentPath { get; set; }
        public IFormFile? DocumentFile { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
