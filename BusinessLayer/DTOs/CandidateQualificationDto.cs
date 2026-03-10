namespace BusinessLayer.DTOs
{
    public class CandidateQualificationDto
    {
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string Qualification { get; set; } = null!;
        public string BoardUniversity { get; set; } = null!;
    }
}
