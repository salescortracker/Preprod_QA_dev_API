

namespace BusinessLayer.DTOs
{
    public class WeekoffDto
    {
        public int WeekoffID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string WeekoffDate { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? UserId { get; set; }
    }
}
