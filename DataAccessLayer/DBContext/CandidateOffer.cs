using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CandidateOffer
{
    public int OfferId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int CandidateId { get; set; }

    public decimal OfferedCtc { get; set; }

    public DateOnly ExpectedDoj { get; set; }

    public string OfferStatus { get; set; } = null!;

    public string Hrname { get; set; } = null!;

    public string? OfferLetterPath { get; set; }

    public string? FilePath { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
