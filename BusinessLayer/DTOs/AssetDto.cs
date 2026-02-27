using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class AssetDto
    {
        public int? AssetID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; }   // ✅ NEW


        public string AssetName { get; set; } = string.Empty;
        public string AssetCode { get; set; } = string.Empty;

        // Optional fields
        public string? AssetLocation { get; set; }
        public decimal AssetCost { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string? AssetDescription { get; set; }
        public string? AssetModel { get; set; }
        public string? PurchaseOrder { get; set; }

        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }

        // ✅ Optional field, not mandatory
        public DateTime? AssetReturnDate { get; set; }

        public int AssetStatusID { get; set; }

        // Nullable audit fields
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // ✅ New fields for reporting manager and approval workflow
        public int? ReportingTo { get; set; } // manager's UserID
        public string? ApprovalStatus { get; set; } // e.g., "Pending", "Approved", "Rejected" 

    }
}
