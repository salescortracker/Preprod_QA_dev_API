using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeEmergencyContact
{
    public int EmergencyContactId { get; set; }

    public string ContactName { get; set; } = null!;

    public int RelationshipId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AlternatePhone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? CompanyId { get; set; }

    public int? RegionId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int UserId { get; set; }

    public virtual Relationship Relationship { get; set; } = null!;
}
