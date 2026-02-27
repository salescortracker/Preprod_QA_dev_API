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
    public class MaritalStatusService: IMaritalStatusService
    {
        private readonly HRMSContext _context;

        public MaritalStatusService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<MaritalStatusDto>> GetAllAsync(int UserId)
        {
            return await _context.MaritalStatuses
                .Where(x =>x.UserId==UserId )
                .OrderByDescending(x => x.MaritalStatusId)
                .Select(x => new MaritalStatusDto
                {
                    MaritalStatusId = x.MaritalStatusId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    MaritalStatusName = x.MaritalStatusName,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(MaritalStatusDto dto)
        {
            var entity = new MaritalStatus
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                MaritalStatusName = dto.MaritalStatusName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                IsDeleted = false,
                UserId=dto.UserId,
                CreatedBy = dto.UserId,
                CreatedAt = DateTime.Now
            };

            _context.MaritalStatuses.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(MaritalStatusDto dto)
        {
            var entity = await _context.MaritalStatuses
                .FirstOrDefaultAsync(x => x.MaritalStatusId == dto.MaritalStatusId && !x.IsDeleted);

            if (entity == null) return false;

            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.MaritalStatusName = dto.MaritalStatusName;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = dto.UserId;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.MaritalStatuses
                .FirstOrDefaultAsync(x => x.MaritalStatusId == id && !x.IsDeleted);

            if (entity == null) return false;

            entity.IsDeleted = true;
            
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
