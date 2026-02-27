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
    public class AssetStatusService:IAssetStatusService
    {
        private readonly HRMSContext _context;

        public AssetStatusService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<AssetStatusDto>> GetAllAsync(int companyId, int regionId)
        {
            return await _context.AssetStatuses
                .Where(x => !x.IsDeleted && x.CompanyId == companyId && x.RegionId == regionId)
                .OrderBy(x => x.AssetStatusName)
                .Select(x => new AssetStatusDto
                {
                    AssetStatusId = x.AssetStatusId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    AssetStatusName = x.AssetStatusName,
                    Description = x.Description,
                    IsActive = x.IsActive,
                })
                .ToListAsync();
        }

        public async Task<int> CreateAsync(AssetStatusDto dto)
        {
            // Validate FK references
            var companyExists = await _context.Companies.AnyAsync(c => c.CompanyId == dto.CompanyId);
            var regionExists = await _context.Regions.AnyAsync(r => r.RegionId == dto.RegionId);

            if (!companyExists || !regionExists)
                throw new Exception("Invalid CompanyId or RegionId. Ensure they exist in the database.");

            var entity = new AssetStatus
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                AssetStatusName = dto.AssetStatusName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
            };

            _context.AssetStatuses.Add(entity);
            await _context.SaveChangesAsync();

            return entity.AssetStatusId;
        }

        public async Task<bool> UpdateAsync(AssetStatusDto dto)
        {
            var entity = await _context.AssetStatuses
                .FirstOrDefaultAsync(x => x.AssetStatusId == dto.AssetStatusId && !x.IsDeleted);

            if (entity == null)
                return false;

            entity.AssetStatusName = dto.AssetStatusName;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int assetStatusId)
        {
            var entity = await _context.AssetStatuses
                .FirstOrDefaultAsync(x => x.AssetStatusId == assetStatusId && !x.IsDeleted);

            if (entity == null)
                return false;

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
