using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeEmergencyContactDto
    {
        public int EmergencyContactId { get; set; }

        public int UserId { get; set; }              // if you use UserId like Family
        public string ContactName { get; set; } = string.Empty;
        public int RelationshipId { get; set; }
        public string? Relationship { get; set; }    // friendly name from Relationships master
        public string? PhoneNumber { get; set; }
        public string? AlternatePhone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? CompanyId { get; set; }
        public int? RegionId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
