namespace BusinessLayer.DTOs
{
    public class ScreeningResultDto
    {
        public int ScreeningResultID { get; set; }
        public int? UserId { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string? Weekoff { get; set; }
        public bool IsActive { get; set; }
       
    }
}
