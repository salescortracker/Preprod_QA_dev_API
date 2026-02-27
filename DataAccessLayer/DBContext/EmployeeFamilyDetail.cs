using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeFamilyDetail
{
    public int FamilyId { get; set; }

    public string Name { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? Occupation { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool IsDependent { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int? GenderId { get; set; }

    public int? RelationshipId { get; set; }

    public int? UserId { get; set; }
}
