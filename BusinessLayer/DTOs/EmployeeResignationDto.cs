using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeResignationDto
    {
        public int ResignationId { get; set; }

        public string? EmployeeId { get; set; }

        public string? ResignationType { get; set; }

        public string? NoticePeriod { get; set; }

        public DateOnly? LastWorkingDay { get; set; }

        public string? ResignationReason { get; set; }

        public string? Status { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? CompanyId { get; set; }

        public int? RegionId { get; set; }

        public int? UserId { get; set; }

        public int? RoleId { get; set; }

        public string? ManagerReason { get; set; }
        public DateTime? ManagerApprovedDate { get; set; }
        public DateTime? ManagerRejectedDate { get; set; }
        public string? HRReason { get; set; }
        public DateTime? HRApprovedDate { get; set; }
        public DateTime? HRRejectedDate { get; set; }
    }
}
