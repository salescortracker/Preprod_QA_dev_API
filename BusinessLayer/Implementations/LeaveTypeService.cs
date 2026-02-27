using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class LeaveTypeService:ILeaveTypeService
    {
        private readonly HRMSContext _context;

        public LeaveTypeService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveTypeDto>> GetLeaveTypesAsync()
        {
            return await (
                from lt in _context.LeaveTypes
                join c in _context.Companies on lt.CompanyId equals c.CompanyId
                join r in _context.Regions on lt.RegionId equals r.RegionId
                where !lt.IsDeleted
                select new LeaveTypeDto
                {
                    LeaveTypeID = lt.LeaveTypeId,
                    CompanyID = lt.CompanyId,
                    RegionID = lt.RegionId,
                    LeaveTypeName = lt.LeaveTypeName,
                    Description = lt.Description,
                    LeaveDays = lt.LeaveDays,
                    IsActive = lt.IsActive,

                    CompanyName = c.CompanyName,
                    RegionName = r.RegionName
                }
            ).ToListAsync();
        }
        public async Task<ApiResponse<IEnumerable<LeaveTypeDto>>> GetCRLeaveTypesAsync(
    int companyId,
    int regionId)
        {
            var list = await (
                from lt in _context.LeaveTypes
                join c in _context.Companies on lt.CompanyId equals c.CompanyId
                join r in _context.Regions on lt.RegionId equals r.RegionId
                where !lt.IsDeleted
                      && lt.CompanyId == companyId
                      && lt.RegionId == regionId
                select new LeaveTypeDto
                {
                    LeaveTypeID = lt.LeaveTypeId,
                    CompanyID = lt.CompanyId,
                    RegionID = lt.RegionId,
                    LeaveTypeName = lt.LeaveTypeName,
                    Description = lt.Description,
                    LeaveDays = lt.LeaveDays,
                    IsActive = lt.IsActive,

                    CompanyName = c.CompanyName,
                    RegionName = r.RegionName
                }
            ).ToListAsync();

            return new ApiResponse<IEnumerable<LeaveTypeDto>>(list);
        }


        public async Task<bool> CreateLeaveTypeAsync(LeaveTypeDto dto)
        {
            var entity = new LeaveType
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                LeaveTypeName = dto.LeaveTypeName,
                Description = dto.Description,
                LeaveDays = dto.LeaveDays,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.Now
            };

            _context.LeaveTypes.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateLeaveTypeAsync(LeaveTypeDto dto)
        {
            var entity = await _context.LeaveTypes
                .FirstOrDefaultAsync(x => x.LeaveTypeId == dto.LeaveTypeID && !x.IsDeleted);

            if (entity == null) return false;

            entity.LeaveTypeName = dto.LeaveTypeName;
            entity.Description = dto.Description;
            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;
            entity.LeaveDays = dto.LeaveDays;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLeaveTypeAsync(int id)
        {
            var entity = await _context.LeaveTypes
                .FirstOrDefaultAsync(x => x.LeaveTypeId == id && !x.IsDeleted);

            if (entity == null)
                return false; // Already deleted or not found

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
