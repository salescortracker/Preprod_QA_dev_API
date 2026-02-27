using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeKpiItemDto
    {
        public int KpiItemId { get; set; }

        public int KpiId { get; set; }

        public string? KpiObjective { get; set; }
        public int? Weightage { get; set; }
        public string? Target { get; set; }
        public string? TaskCompleted { get; set; }
        public int? SelfRating { get; set; }
        public string? Remarks { get; set; }

        // Auditing
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
