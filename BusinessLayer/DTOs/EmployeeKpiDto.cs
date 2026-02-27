using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeKpiDto
    {
        public int KpiId { get; set; }

        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }

        public string EmployeeNameId { get; set; } = string.Empty;
        public int ReportingManagerId { get; set; }
        public string? Designation { get; set; }
        public string? DepartmentId { get; set; }
        public DateOnly? DateOfJoining { get; set; }
        public string? ProbationStatus { get; set; }

        public string? PerformanceCycle { get; set; }
        public DateOnly? ApplicableStartDate { get; set; }
        public DateOnly? ApplicableEndDate { get; set; }

        public string? ProgressType { get; set; }
        public int? AppraisalYear { get; set; }
        public string? DocumentEvidencePath { get; set; }

        public string? SelfReviewSummary { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Auditing
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string EmployeeEmail { get; set; } = string.Empty;

        public List<EmployeeKpiItemDto> KpiItems { get; set; } = new List<EmployeeKpiItemDto>();
    }
}
