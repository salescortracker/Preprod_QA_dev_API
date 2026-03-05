

namespace BusinessLayer.DTOs
{
    public class CandidateExperienceDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Designation { get; set; } = null!;
        public string Organization { get; set; } = null!;
    }
}
