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
    public class ShiftAllocationService: IShiftAllocationService
    {
        private readonly HRMSContext _context;

        public ShiftAllocationService(HRMSContext context)
        {
            _context = context;
        }

        // ======================================================
        //              SHIFT MASTER SERVICES
        // ======================================================

        public async Task<IEnumerable<ShiftMasterDto>> GetAllShiftsAsync(int userId)
        {
            return await _context.ShiftMasters
                .Where(x => x.UserId == userId)
                .Select(x => new ShiftMasterDto
                {
                    ShiftID = x.ShiftId,
                    ShiftName = x.ShiftName,
                    ShiftStartTime = x.ShiftStartTime.ToString("HH:mm"),
                    ShiftEndTime = x.ShiftEndTime.ToString("HH:mm"),
                    GraceTime = x.GraceTime,
                    CompanyName = x.CompanyId != null ? _context.Companies.Where(c => c.CompanyId == x.CompanyId).FirstOrDefault().CompanyName : null,
                    RegionName = x.RegionId != null ? _context.Regions.Where(r => r.RegionId == x.RegionId).FirstOrDefault().RegionName : null,
                    IsActive = x.IsActive,
                    CompanyID = x.CompanyId,
                    RegionID = x.RegionId,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    ModifiedAt = x.ModifiedAt,
                    ModifiedBy = x.ModifiedBy,
                    UserId = x.UserId
                }).ToListAsync();
        }

        public async Task<ShiftMasterDto?> GetShiftByIdAsync(int shiftId)
        {
            return await _context.ShiftMasters
                .Where(x => x.ShiftId == shiftId)
                .Select(x => new ShiftMasterDto
                {
                    ShiftID = x.ShiftId,
                    ShiftName = x.ShiftName,
                    ShiftStartTime = x.ShiftStartTime.ToString("HH:mm"),
                    ShiftEndTime = x.ShiftEndTime.ToString("HH:mm"),
                    GraceTime = x.GraceTime,
                    IsActive = x.IsActive,
                    CompanyID = x.CompanyId,
                    RegionID = x.RegionId,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    ModifiedAt = x.ModifiedAt,
                    ModifiedBy = x.ModifiedBy,
                    UserId = x.UserId
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> AddShiftAsync(ShiftMasterDto dto)
        {
            try
            {
                if (!TimeOnly.TryParse(dto.ShiftStartTime, out var startTime))
                    throw new ArgumentException("Invalid ShiftStartTime");

                if (!TimeOnly.TryParse(dto.ShiftEndTime, out var endTime))
                    throw new ArgumentException("Invalid ShiftEndTime");

                var entity = new ShiftMaster
                {
                    ShiftName = dto.ShiftName,
                    ShiftStartTime = startTime,
                    ShiftEndTime = endTime,
                    GraceTime = dto.GraceTime,
                    CompanyId = dto.CompanyID,
                    RegionId = dto.RegionID,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.Now,
                    CreatedBy = dto.UserId,
                    UserId = dto.UserId
                };

                _context.ShiftMasters.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> UpdateShiftAsync(ShiftMasterDto dto)
        {
            var entity = await _context.ShiftMasters.FindAsync(dto.ShiftID);
            if (entity == null) return false;

            entity.ShiftName = dto.ShiftName;
            entity.ShiftStartTime = TimeOnly.Parse(dto.ShiftStartTime);
            entity.ShiftEndTime = TimeOnly.Parse(dto.ShiftEndTime);
            entity.GraceTime = dto.GraceTime;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.UserId = dto.UserId;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteShiftAsync(int shiftId)
        {
            var entity = await _context.ShiftMasters.FindAsync(shiftId);
            if (entity == null) return false;

            _context.ShiftMasters.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActivateShiftAsync(int shiftId)
        {
            var entity = await _context.ShiftMasters.FindAsync(shiftId);
            if (entity == null) return false;

            entity.IsActive = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeactivateShiftAsync(int shiftId)
        {
            var entity = await _context.ShiftMasters.FindAsync(shiftId);
            if (entity == null) return false;

            entity.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<ShiftMasterDto>> GetShiftsForDropdownAsync(int companyId, int regionId)
        {
            return await _context.ShiftMasters
                .Where(x => x.CompanyId == companyId && x.RegionId == regionId && x.IsActive)
                .Select(x => new ShiftMasterDto
                {
                    ShiftID = x.ShiftId,
                    ShiftName = x.ShiftName,
                    ShiftStartTime = x.ShiftStartTime.ToString("HH:mm"),
                    ShiftEndTime = x.ShiftEndTime.ToString("HH:mm")
                })
                .ToListAsync();
        }

        // ======================================================
        //              SHIFT ALLOCATION SERVICES
        // ======================================================

        public async Task<IEnumerable<ShiftAllocationDto>> GetAllAllocationsAsync(int userId)
        {
            return await (
                from sa in _context.ShiftAllocations
                where sa.UserId == userId
                join u in _context.Users
                    on sa.UserId equals u.UserId into userGroup
                from u in userGroup.DefaultIfEmpty()

                join sm in _context.ShiftMasters
                    on sa.ShiftId equals sm.ShiftId into shiftGroup
                from sm in shiftGroup.DefaultIfEmpty()

                select new ShiftAllocationDto
                {
                    ShiftAllocationId = sa.ShiftAllocationId,
                    UserID = sa.UserId,
                    EmployeeCode = sa != null ? sa.EmployeeCode : "",
                    FullName = sa != null ? sa.FullName : "",
                    CompanyID = sa.CompanyId,
                    RegionID = sa.RegionId,
                    ShiftID = sa.ShiftId ?? 0,
                    ShiftName = sm != null ? sm.ShiftName : "",

                    StartDate = sa.StartDate,
                    EndDate = sa.EndDate,

                    IsActive = sa.IsActive,
                    CreatedBy = sa.CreatedBy,
                    CreatedDate = sa.CreatedDate,
                    ModifiedBy = sa.ModifiedBy,
                    ModifiedDate = sa.ModifiedDate
                }
            ).ToListAsync();
        }



        public async Task<ShiftAllocationDto?> GetAllocationByIdAsync(int id)
        {
            return await (
                from sa in _context.ShiftAllocations
                join u in _context.Users on sa.UserId equals u.UserId
                join sm in _context.ShiftMasters on sa.ShiftId equals sm.ShiftId
                where sa.ShiftAllocationId == id
                select new ShiftAllocationDto
                {
                    ShiftAllocationId = sa.ShiftAllocationId,
                    UserID = sa.UserId,
                    EmployeeCode = u.EmployeeCode,
                    FullName = u.FullName,
                    CompanyID = sa.CompanyId,
                    RegionID = sa.RegionId,
                    ShiftID = sa.ShiftId ?? 0,
                    ShiftName = sm.ShiftName,
                    StartDate = sa.StartDate,
                    EndDate = sa.EndDate,
                    IsActive = sa.IsActive,
                    CreatedBy = sa.CreatedBy,
                    CreatedDate = sa.CreatedDate,
                    ModifiedBy = sa.ModifiedBy,
                    ModifiedDate = sa.ModifiedDate
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<bool> AllocateShiftAsync(ShiftAllocationDto dto)
        {
            var entity = new ShiftAllocation
            {
                UserId = dto.UserID,
                ShiftId = dto.ShiftID,
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                EmployeeCode = dto.EmployeeCode,
                FullName = dto.FullName,
                ShiftName = dto.ShiftName,

                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate
            };

            _context.ShiftAllocations.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAllocationAsync(ShiftAllocationDto dto)
        {
            var entity = await _context.ShiftAllocations.FindAsync(dto.ShiftAllocationId);
            if (entity == null) return false;

            entity.UserId = dto.UserID;
            entity.FullName = dto.FullName;
            entity.EmployeeCode = dto.EmployeeCode;
            entity.ShiftId = dto.ShiftID;  // FIXED
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAllocationAsync(int id)
        {
            var entity = await _context.ShiftAllocations.FindAsync(id);
            if (entity == null) return false;

            _context.ShiftAllocations.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        // ======================================================
        //              USER DETAILS
        // ======================================================

        public async Task<UserReadDto?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Where(x => x.UserId == userId)
                .Select(x => new UserReadDto
                {
                    UserID = x.UserId,
                    EmployeeCode = x.EmployeeCode,
                    FullName = x.FullName,
                    Email = x.Email,
                    CompanyID = x.CompanyId,    // Added
                    RegionID = x.RegionId,
                    Status = x.Status,
                    RoleName = x.RoleId.ToString(),
                    CreatedDate = (DateTime)x.CreatedDate
                }).FirstOrDefaultAsync();
        }
        public async Task<EmployeeShiftDto?> GetEmployeeShiftByEmployeeCodeAsync(string employeeCode)
        {
            return await (
                from s in _context.ShiftAllocations
                join sm in _context.ShiftMasters
                    on s.ShiftId equals sm.ShiftId
                where s.EmployeeCode == employeeCode
                select new EmployeeShiftDto
                {
                    ShiftName = sm.ShiftName,
                    ShiftStartTime = sm.ShiftStartTime,
                    ShiftEndTime = sm.ShiftEndTime
                }
            ).FirstOrDefaultAsync();
        }
    }
}
