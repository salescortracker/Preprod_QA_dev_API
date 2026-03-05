using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CompanyPolicyMasterDto
    {
        public int PolicyId { get; set; }

        public string? PolicyTitle { get; set; }
        public string? PolicyDescription { get; set; }

        public DateOnly? PostedDate { get; set; }
        public DateOnly? EffectiveDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        public bool? IsActive { get; set; }

        public int? UserId { get; set; }
        public int? CompanyId { get; set; }
        public int? RegionId { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Category { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public string? AttachmentName { get; set; }

        public string? AttachmentPath { get; set; }

        public int? DepartmentId { get; set; }
    }
}
