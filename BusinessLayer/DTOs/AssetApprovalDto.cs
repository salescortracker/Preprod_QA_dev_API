using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class AssetApprovalDto
    {
        public int AssetID { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public string AssetCode { get; set; } = string.Empty;
        public string AssetLocation { get; set; } = string.Empty;
        public decimal AssetCost { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty; // Pending / Approved / Rejected
        public string EmployeeName { get; set; } = string.Empty;
    }
}
