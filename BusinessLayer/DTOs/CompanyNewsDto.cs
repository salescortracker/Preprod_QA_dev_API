namespace BusinessLayer.DTOs
{
    public class CompanyNewsDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string? AttachmentPath { get; set; }
        public string? AttachmentName { get; set; }
        public bool IsActive { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
    }
}
