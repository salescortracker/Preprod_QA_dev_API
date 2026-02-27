using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class MissedPunchService: IMissedPunchService
    {
        private readonly HRMSContext _context;

        public MissedPunchService(HRMSContext context)
        {
            _context = context;
        }

        // 🔹 CREATE
        public async Task<MissedPunchRequest> CreateMissedPunchRequest(
            CreateMissedPunchRequestDto dto)
        {
            try
            {
                var entity = new MissedPunchRequest
                {
                    EmployeeId = dto.EmployeeID,
                    MissedDate = dto.MissedDate,
                    MissedType = dto.MissedType,
                    ManagerId=dto.reportingTo,
                    CorrectClockIn = dto.CorrectClockIn,
                    CorrectClockOut = dto.CorrectClockOut,
                    Reason = dto.Reason,
                    Status = "Pending",
                    CompanyId = dto.CompanyID,
                    RegionId = dto.RegionID,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId,
                    UserId = dto.UserId
                };

                _context.MissedPunchRequests.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // 🔹 EMPLOYEE LIST
        public async Task<IEnumerable<MissedPunchRequest>> GetMissedPunchRequest(
            int companyId, int? regionId)
        {
            return await _context.MissedPunchRequests
                .Where(x =>
                    x.CompanyId == companyId &&
                    (regionId == null || x.RegionId == regionId))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // 🔹 MANAGER APPROVAL LIST
        public async Task<IEnumerable<MissedPunchRequest>> GetApprovalMissedPunchRequest(
            int companyId, int? regionId, int managerId)
        {
            var result= await _context.MissedPunchRequests
                .Where(x =>
                   
                    x.Status == "Pending" && x.ManagerId==managerId)
                .OrderBy(x => x.MissedDate)
                .ToListAsync();

            return result;
        }

        // 🔹 SINGLE APPROVE / REJECT
        public async Task<bool> UpdateMissedPunch(UpdateMissedPunchDto dto)
        {
            var entity = await _context.MissedPunchRequests
                .FirstOrDefaultAsync(x =>
                    x.MissedPunchRequestId == dto.MissedPunchRequestID &&
                    x.CompanyId == dto.CompanyID &&
                    (dto.RegionID == null || x.RegionId == dto.RegionID) &&
                    x.Status == "Pending");

            if (entity == null)
                return false;

            entity.Status = dto.Status;
            entity.ManagerRemarks = dto.ManagerRemarks;
            entity.ManagerId = dto.ManagerID;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.ManagerID;

            await _context.SaveChangesAsync();
            return true;
        }

        // 🔥 BULK APPROVE / REJECT
        public async Task<int> BulkApproveRejectPunch(BulkApproveRejectPunchDto dto)
        {
            var records = await _context.MissedPunchRequests
                .Where(x =>
                    dto.MissedPunchRequestIds.Contains(x.MissedPunchRequestId) &&
                  
                    x.Status == "Pending")
                .ToListAsync();

            if (!records.Any())
                return 0;

            foreach (var item in records)
            {
                item.Status = dto.Status;
                item.ManagerRemarks = dto.ManagerRemarks;
                item.ManagerId = dto.ManagerID;
                item.ModifiedAt = DateTime.UtcNow;
                item.ModifiedBy = dto.ManagerID;
            }

            await _context.SaveChangesAsync();
            return records.Count;
        }
    }
}
