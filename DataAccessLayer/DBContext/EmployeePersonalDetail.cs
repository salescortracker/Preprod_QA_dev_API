using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeePersonalDetail
{
    public int Id { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int GenderId { get; set; }

    public string MobileNumber { get; set; } = null!;

    public string PersonalEmail { get; set; } = null!;

    public string PermanentAddress { get; set; } = null!;

    public string PresentAddress { get; set; } = null!;

    public string Pannumber { get; set; } = null!;

    public string AadhaarNumber { get; set; } = null!;

    public byte[]? ProfilePictureBinary { get; set; }

    public string? ProfilePictureBase64 { get; set; }

    public string? ProfilePictureName { get; set; }

    public string? PassportNumber { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? Uan { get; set; }

    public string BloodGroup { get; set; } = null!;

    public string? Citizenship { get; set; }

    public string? Religion { get; set; }

    public string? DrivingLicence { get; set; }

    public int MaritalStatusId { get; set; }

    public DateOnly? MarriageDate { get; set; }

    public string? WorkPhone { get; set; }

    public string? LinkedInProfile { get; set; }

    public string? PreviousExperienceText { get; set; }

    public byte? PreviousExperienceYears { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? BandGrade { get; set; }

    public string? EsicNumber { get; set; }

    public string? Pfnumber { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public string? EmployeeType { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual Gender Gender { get; set; } = null!;

    public virtual MaritalStatus MaritalStatus { get; set; } = null!;
}
