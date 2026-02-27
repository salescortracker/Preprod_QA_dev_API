using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class AssetApprovalService:IAssetApprovalService
    {
        private readonly HRMSContext _context;

        public AssetApprovalService(HRMSContext context)
        {
            _context = context;
        }

        // 🔹 Manager sees team pending assets
        public async Task<List<AssetApprovalDto>> GetPendingAssetsForManagerAsync(int managerUserId)
        {
            return await _context.Assets
                .Where(a => a.ReportingTo == managerUserId &&
                            a.ApprovalStatus == "Pending")
                .Select(a => new AssetApprovalDto
                {
                    AssetID = a.AssetId,
                    AssetName = a.AssetName,
                    AssetCode = a.AssetCode,
                    AssetLocation = a.AssetLocation,
                    AssetCost = a.AssetCost,
                    CurrencyCode = a.CurrencyCode,
                    ApprovalStatus = a.ApprovalStatus,
                    EmployeeName = a.EmployeeName
                })
                .OrderByDescending(a => a.AssetID)
                .ToListAsync();
        }

        // 🔹 Single API → Approve / Reject
        public async Task<bool> ApproveOrRejectAssetAsync(
            int assetId,
            int managerUserId,
            string action)
        {
            var asset = await _context.Assets
                .FirstOrDefaultAsync(a =>
                    a.AssetId == assetId &&
                    a.ReportingTo == managerUserId &&
                    a.ApprovalStatus == "Pending");

            if (asset == null)
                return false;

            if (action == "Approve")
                asset.ApprovalStatus = "Approved";
            else if (action == "Reject")
                asset.ApprovalStatus = "Rejected";
            else
                throw new Exception("Invalid action");

            asset.ApprovedBy = managerUserId;
            asset.ApprovedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
