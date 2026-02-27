using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class DigitalCardDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EmployeeCode { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public string RegionName { get; set; }
        public string Location { get; set; }
        public string MobileNumber { get; set; }
        public string PersonalEmail { get; set; }
        public string LinkedInProfile { get; set; }
        public string ProfilePictureBase64 { get; set; }
    }
}
