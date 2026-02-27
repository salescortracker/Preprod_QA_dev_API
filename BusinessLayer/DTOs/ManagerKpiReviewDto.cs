using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ManagerKpiReviewDto
    {
        public int ReviewId { get; set; }
        public int KPIItemId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public int AppraisalYear { get; set; }

        // From EmployeeKPIItems
        public string KPIObjective { get; set; }
        public decimal Weightage { get; set; }
        public string Target { get; set; }
        public string TaskCompleted { get; set; }
        public int SelfRating { get; set; }

        // From ManagerKPIReview
        public int? ManagerRating { get; set; }
        public decimal? AvgRating { get; set; }
        public string ManagerComments { get; set; }
        public string Status { get; set; } = "Pending";
        public string EmployeeEmail { get; set; } = string.Empty;

        public int UserId { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int ManagerId { get; set; }
    }
}
