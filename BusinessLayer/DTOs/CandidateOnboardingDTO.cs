using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateOnboardingDTO
    {
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int CandidateId { get; set; }

        public DateTime? JoiningDate { get; set; }
        public bool DocumentsCollected { get; set; }
        public string BackgroundCheckStatus { get; set; } = "Pending";
        public bool LaptopIssued { get; set; }
        public string? BuddyAssigned { get; set; }
    }
}
