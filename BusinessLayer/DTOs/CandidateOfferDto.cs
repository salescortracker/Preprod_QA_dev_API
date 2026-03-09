using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateOfferDto
    {
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int CandidateId { get; set; }

        public decimal OfferedCtc { get; set; }
        public DateTime ExpectedDoj { get; set; }
        public string OfferStatus { get; set; } = null!;
        public string HrName { get; set; } = null!;
        public string? OfferLetterPath { get; set; }
        public string? FilePath { get; set; }

        // For listing
        public int OfferId { get; set; }
        public string? SeqNo { get; set; }
        public string? CandidateName { get; set; }
        public string? Designation { get; set; }
        public int StageId { get; set; }
        public string? Email { get; set; }

    }
}
