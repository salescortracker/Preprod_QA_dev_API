using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class PersonalDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int genderId { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalEmail { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string PanNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public string PassportNumber { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Uan { get; set; }
        public string BloodGroup { get; set; }
        public string Citizenship { get; set; }
        public string Religion { get; set; }
        public string DrivingLicence { get; set; }
        public int maritalStatusId { get; set; }
        public DateOnly? MarriageDate { get; set; }
        public string WorkPhone { get; set; }
        public string LinkedInProfile { get; set; }
        public string PreviousExperience { get; set; }
        public IFormFile? profilePicture { get; set; }
        public string? profilePicturePath { get; set; }
        public string brandGrade { get; set; }
        public string? esicNumber { get; set; }
        public string? pfNumber { get; set; }
        public string? employmentType { get; set; }
        public DateTime? dateofJoining { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public int userId { get; set; }
    }
}
