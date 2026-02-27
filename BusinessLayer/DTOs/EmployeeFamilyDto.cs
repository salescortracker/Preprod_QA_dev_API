using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeFamilyDto
    {
        public int FamilyId { get; set; }
        public int? UserId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }

        public string Name { get; set; } = string.Empty;
        public int? RelationshipId { get; set; }              // FK to adminmaster.Relationship
        public string? Relationship { get; set; }             // Relationship name (from Relationship table)
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int? GenderId { get; set; }

        public string? Occupation { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDependent { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
