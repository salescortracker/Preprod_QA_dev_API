namespace BusinessLayer.DTOs
{
    public class BulkApproveRejectWorkFromHomeDto
    {
        public List<int> WFHRequestIDs { get; set; } = new();
        public string Status { get; set; }
        public string? ManagerRemarks { get; set; }
        public int ManagerID { get; set; }
        public int CompanyID { get; set; }
    }
}
