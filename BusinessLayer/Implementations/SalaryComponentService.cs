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
    public class SalaryComponentService: ISalaryComponentService
    {
        private readonly HRMSContext _context;

        public SalaryComponentService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<SalaryComponentDto>> GetAllAsync(int userId)
        {
            return await _context.SalaryComponents
                .Where(x => x.UserId == userId)
                .Select(x => new SalaryComponentDto
                {
                    ComponentId = x.ComponentId,
                    ComponentName = x.ComponentName,
                    Type = x.Type,
                    CalculationType = x.CalculationType,
                    PercentageOf = x.PercentageOf,
                    IsTaxable = x.IsTaxable,
                    IsActive = x.IsActive,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = x.CreatedBy,
                    ModifiedAt = DateTime.UtcNow,
                    ModifiedBy = x.ModifiedBy,
                })
                .ToListAsync();
        }

        public async Task<SalaryComponentDto?> GetByIdAsync(int id, int userId)
        {
            var entity = await _context.SalaryComponents
                .FirstOrDefaultAsync(x => x.ComponentId == id && x.UserId == userId);

            if (entity == null) return null;

            return new SalaryComponentDto
            {
                ComponentId = entity.ComponentId,
                ComponentName = entity.ComponentName,
                Type = entity.Type,
                CalculationType = entity.CalculationType,
                PercentageOf = entity.PercentageOf,
                IsTaxable = entity.IsTaxable,
                IsActive = entity.IsActive,
                CompanyId = entity.CompanyId,
                RegionId = entity.RegionId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = entity.CreatedBy,
                ModifiedAt = DateTime.UtcNow,
                ModifiedBy = entity.ModifiedBy,
            };
        }

        public async Task<SalaryComponentDto> CreateAsync(SalaryComponentDto dto, int userId)
        {
            var entity = new SalaryComponent
            {
                ComponentName = dto.ComponentName,
                Type = dto.Type,
                CalculationType = dto.CalculationType,
                PercentageOf = dto.PercentageOf,
                IsTaxable = dto.IsTaxable,
                IsActive = true,
                UserId = userId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.CreatedBy,
                ModifiedAt = DateTime.UtcNow,
                ModifiedBy = dto.ModifiedBy,
            };

            _context.SalaryComponents.Add(entity);
            await _context.SaveChangesAsync();

            dto.ComponentId = entity.ComponentId;
            return dto;
        }

        public async Task<SalaryComponentDto?> UpdateAsync(int id, SalaryComponentDto dto, int userId)
        {
            var entity = await _context.SalaryComponents
                .FirstOrDefaultAsync(x => x.ComponentId == id && x.UserId == userId);

            if (entity == null) return null;

            entity.ComponentName = dto.ComponentName;
            entity.Type = dto.Type;
            entity.CalculationType = dto.CalculationType;
            entity.PercentageOf = dto.PercentageOf;
            entity.IsTaxable = dto.IsTaxable;
            entity.IsActive = dto.IsActive;
            entity.UserId = userId;
            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.CreatedBy = dto.CreatedBy;
            entity.CreatedAt = dto.CreatedAt;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var entity = await _context.SalaryComponents
                .FirstOrDefaultAsync(x => x.ComponentId == id && x.UserId == userId);

            if (entity == null) return false;

            _context.SalaryComponents.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    

}
}
