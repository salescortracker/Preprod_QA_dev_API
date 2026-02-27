using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class AssetService:IAssetService
    {
        private readonly HRMSContext _context;

        public AssetService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAssetAsync(AssetDto assetDto)
        {
            // Get reporting manager for selected employee
            var employee = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == assetDto.UserID);

            if (employee == null)
                throw new Exception("Selected employee not found");

            var reportingTo = employee.ReportingTo;

            // Create asset for employee
            var asset = new Asset
            {
                CompanyId = assetDto.CompanyID,
                RegionId = assetDto.RegionID,
                UserId = assetDto.UserID,
                EmployeeName = employee.FullName,  // ✅ Save employee name from DB
                AssetName = assetDto.AssetName,
                AssetCode = assetDto.AssetCode,
                AssetLocation = assetDto.AssetLocation,
                AssetCost = assetDto.AssetCost,
                CurrencyCode = assetDto.CurrencyCode,
                AssetDescription = assetDto.AssetDescription,
                AssetModel = assetDto.AssetModel,
                PurchaseOrder = assetDto.PurchaseOrder,
                WarrantyStartDate = assetDto.WarrantyStartDate.HasValue
                    ? DateOnly.FromDateTime(assetDto.WarrantyStartDate.Value)
                    : null,
                WarrantyEndDate = assetDto.WarrantyEndDate.HasValue
                    ? DateOnly.FromDateTime(assetDto.WarrantyEndDate.Value)
                    : null,
                AssetReturnDate = assetDto.AssetReturnDate.HasValue
                    ? DateOnly.FromDateTime(assetDto.AssetReturnDate.Value)
                    : null,
                AssetStatusId = assetDto.AssetStatusID,
                CreatedAt = assetDto.CreatedAt ?? DateTime.Now,
                CreatedBy = assetDto.CreatedBy,
                ReportingTo = assetDto.ReportingTo,
                ApprovalStatus = "Pending" // default pending for manager approval
            };

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            // TODO: Optionally trigger email to reporting manager here

            return asset.AssetId;
        }

        public async Task<bool> UpdateAssetAsync(AssetDto assetDto)
        {
            var asset = await _context.Assets.FindAsync(assetDto.AssetID);
            if (asset == null) return false;

            // Update employee name if UserID changed
            var employee = await _context.Users.FirstOrDefaultAsync(u => u.UserId == assetDto.UserID);
            if (employee != null)
            {
                asset.EmployeeName = employee.FullName;
                asset.UserId = employee.UserId;
                asset.ReportingTo = employee.ReportingTo;
            }

            asset.AssetName = assetDto.AssetName;
            asset.AssetCode = assetDto.AssetCode;
            asset.AssetLocation = assetDto.AssetLocation;
            asset.AssetCost = assetDto.AssetCost;
            asset.CurrencyCode = assetDto.CurrencyCode;
            asset.AssetDescription = assetDto.AssetDescription;
            asset.AssetModel = assetDto.AssetModel;
            asset.PurchaseOrder = assetDto.PurchaseOrder;
            asset.WarrantyStartDate = assetDto.WarrantyStartDate.HasValue
                ? DateOnly.FromDateTime(assetDto.WarrantyStartDate.Value)
                : null;
            asset.WarrantyEndDate = assetDto.WarrantyEndDate.HasValue
                ? DateOnly.FromDateTime(assetDto.WarrantyEndDate.Value)
                : null;
            asset.AssetReturnDate = assetDto.AssetReturnDate.HasValue
                ? DateOnly.FromDateTime(assetDto.AssetReturnDate.Value)
                : null;
            asset.AssetStatusId = assetDto.AssetStatusID;
            asset.ModifiedAt = DateTime.Now;
            asset.ModifiedBy = assetDto.ModifiedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAssetAsync(int assetId)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null) return false;

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AssetDto>> GetAllAssetsAsync()
        {
            return await _context.Assets
                .Select(a => new AssetDto
                {
                    AssetID = a.AssetId,
                    CompanyID = a.CompanyId,
                    RegionID = a.RegionId,
                    UserID = a.UserId,
                    EmployeeName = a.EmployeeName,
                    AssetName = a.AssetName,
                    AssetCode = a.AssetCode,
                    AssetLocation = a.AssetLocation,
                    AssetCost = a.AssetCost,
                    CurrencyCode = a.CurrencyCode,
                    AssetDescription = a.AssetDescription,
                    AssetModel = a.AssetModel,
                    PurchaseOrder = a.PurchaseOrder,
                    WarrantyStartDate = a.WarrantyStartDate.HasValue
                        ? a.WarrantyStartDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    WarrantyEndDate = a.WarrantyEndDate.HasValue
                        ? a.WarrantyEndDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    AssetReturnDate = a.AssetReturnDate.HasValue
                        ? a.AssetReturnDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    AssetStatusID = a.AssetStatusId,
                    ReportingTo = a.ReportingTo,
                    ApprovalStatus = a.ApprovalStatus
                })
                .OrderByDescending(x => x.AssetID)
                .ToListAsync();
        }

        public async Task<List<AssetDto>> GetAssetsByUserIdAsync(int userId)
        {
            return await _context.Assets
                .Where(a => a.UserId == userId)
                .Select(a => new AssetDto
                {
                    AssetID = a.AssetId,
                    CompanyID = a.CompanyId,
                    RegionID = a.RegionId,
                    UserID = a.UserId,
                    EmployeeName = a.EmployeeName,
                    AssetName = a.AssetName,
                    AssetCode = a.AssetCode,
                    AssetLocation = a.AssetLocation,
                    AssetCost = a.AssetCost,
                    CurrencyCode = a.CurrencyCode,
                    AssetDescription = a.AssetDescription,
                    AssetModel = a.AssetModel,
                    PurchaseOrder = a.PurchaseOrder,
                    WarrantyStartDate = a.WarrantyStartDate.HasValue
                        ? a.WarrantyStartDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    WarrantyEndDate = a.WarrantyEndDate.HasValue
                        ? a.WarrantyEndDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    AssetReturnDate = a.AssetReturnDate.HasValue
                        ? a.AssetReturnDate.Value.ToDateTime(TimeOnly.MinValue)
                        : null,
                    AssetStatusID = a.AssetStatusId,
                    ReportingTo = a.ReportingTo,
                    ApprovalStatus = a.ApprovalStatus
                })
                .ToListAsync();
        }

        public async Task<List<AssetStatusDto>> GetAllAssetStatusesAsync()
        {
            return await _context.AssetStatuses
                .Where(x => x.IsActive && !x.IsDeleted)
                .Select(x => new AssetStatusDto
                {
                    AssetStatusId = x.AssetStatusId,
                    AssetStatusName = x.AssetStatusName
                })
                .ToListAsync();
        }
        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await _context.Users
                .Where(u => u.Status == "Active") // optional filter
                .Select(u => new EmployeeDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName
                })
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

    }
}
