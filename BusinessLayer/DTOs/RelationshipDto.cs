namespace BusinessLayer.DTOs
{
    public class RelationshipDto
    {
        public int RelationshipID { get; set; }
        public string RelationshipName { get; set; } = string.Empty;
        public int regionId { get; set; }
        public int companyId { get; set; }
        public int userId { get; set; }
        public bool isActive { get; set; }
    }
}
