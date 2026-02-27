using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeImmigrationDto
    {
        public int ImmigrationId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public int UserId { get; set; }

        public string? EmployeeId { get; set; }


        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? PassportNumber { get; set; }
        public DateOnly? PassportExpiryDate { get; set; }

        public int? VisaTypeId { get; set; }
        public string? VisaTypeName { get; set; }

        public int? StatusId { get; set; }
        public string? StatusName { get; set; }

        public string? VisaNumber { get; set; }
        public string? VisaIssuingCountry { get; set; }
        public DateOnly? VisaIssueDate { get; set; }
        public DateOnly? VisaExpiryDate { get; set; }

        public string? EmployerName { get; set; }
        public string? EmployerAddress { get; set; }
        public string? ContactPerson { get; set; }
        public string? EmployerContact { get; set; }

        public string? Remarks { get; set; }

        public string? PassportCopyPath { get; set; }
        public string? VisaCopyPath { get; set; }
        public string? OtherDocumentsPath { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string? CreatedByName { get; set; }

        public VisaTypeDto? VisaType { get; set; }
        public WorkAuthStatusDto? WorkAuthStatus { get; set; }

        public IFormFile? PassportCopy { get; set; }
        public IFormFile? VisaCopy { get; set; }
        public IFormFile? OtherDocuments { get; set; }
    }
}
