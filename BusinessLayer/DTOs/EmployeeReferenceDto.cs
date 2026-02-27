using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs
{
    public class EmployeeReferenceDto
    {
        public int ReferenceId { get; set; }

        
        public int RegionId { get; set; }

        
        public int CompanyId { get; set; }

       
        public string? Name { get; set; } = string.Empty;

       
        public string? Title { get; set; } = string.Empty;

        
        public string? CompanyName { get; set; } = string.Empty;

       
       
        public string? Email { get; set; } = string.Empty;

       
        public string? MobileNumber { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; } // set by service on add
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }

        public int UserId { get; set; }
    }
}
