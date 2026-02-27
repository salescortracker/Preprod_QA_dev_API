using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class AttendanceStatusService: IAttendanceStatusService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceStatusService(IUnitOfWork unitOfWork, HRMSContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #region Get All
        public async Task<ApiResponse<IEnumerable<AttendanceStatusDto>>>
            GetAllAsync(int companyId, int regionId)
        {
            try
            {
                if (companyId <= 0 || regionId <= 0)
                    return new ApiResponse<IEnumerable<AttendanceStatusDto>>(
                        null, "Invalid company or region id", false);

                var data = await _unitOfWork
                    .Repository<AttendanceStatus>()
                    .GetAllAsync();

                var result = data
                    .Where(x => x.CompanyId == companyId &&
                                x.RegionId == regionId &&
                                !x.IsDeleted)
                    .Select(MapToDto)
                    .ToList();

                return new ApiResponse<IEnumerable<AttendanceStatusDto>>(
                    result, "Attendance statuses fetched successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AttendanceStatusDto>>(
                    null, ex.Message, false);
            }
        }
        #endregion


        #region Get By Id
        public async Task<ApiResponse<AttendanceStatusDto?>>
            GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.AttendanceStatuses
                    .FirstOrDefaultAsync(x => x.AttendanceStatusId == id && !x.IsDeleted);

                if (entity == null)
                    return new ApiResponse<AttendanceStatusDto?>(
                        null, "Attendance status not found", false);

                return new ApiResponse<AttendanceStatusDto?>(
                    MapToDto(entity), "Attendance status fetched successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AttendanceStatusDto?>(
                    null, ex.Message, false);
            }
        }
        #endregion


        #region Create
        public async Task<ApiResponse<AttendanceStatusDto>>
            CreateAsync(AttendanceStatusDto dto)
        {
            try
            {
                // 🔹 Basic Validation
                if (dto == null)
                    return new ApiResponse<AttendanceStatusDto>(
                        null, "Invalid request", false);

                if (string.IsNullOrWhiteSpace(dto.AttendanceStatusName))
                    return new ApiResponse<AttendanceStatusDto>(
                        null, "Attendance status name is required", false);

                var trimmedName = dto.AttendanceStatusName.Trim();

                // 🔥 Optimized Duplicate Check (DB level filtering)
                var exists = await _context.AttendanceStatuses
                    .AnyAsync(x =>
                        x.CompanyId == dto.CompanyId &&
                        x.RegionId == dto.RegionId &&
                        !x.IsDeleted &&
                        x.AttendanceStatusName.ToLower() == trimmedName.ToLower());

                if (exists)
                    return new ApiResponse<AttendanceStatusDto>(
                        null, "Attendance status already exists", true);

                var entity = new AttendanceStatus
                {
                    AttendanceStatusName = trimmedName,
                    CompanyId = dto.CompanyId,
                    RegionId = dto.RegionId,
                    Description = dto.Description,
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedBy = dto.CreatedBy,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<AttendanceStatus>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<AttendanceStatusDto>(
                    MapToDto(entity),
                    "Attendance status created successfully",
                    true);
            }
            catch (DbUpdateException dbEx)
            {
                return new ApiResponse<AttendanceStatusDto>(
                    null, "Database error occurred", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AttendanceStatusDto>(
                    null, "Something went wrong: " + ex.Message, false);
            }
        }
        #endregion


        #region Update
        public async Task<ApiResponse<AttendanceStatusDto>>
            UpdateAsync(AttendanceStatusDto dto)
        {
            try
            {
                var entity = await _unitOfWork
                    .Repository<AttendanceStatus>()
                    .GetByIdAsync(dto.AttendanceStatusId);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<AttendanceStatusDto>(
                        null, "Attendance status not found", false);

                var name = dto.AttendanceStatusName.Trim().ToLower();

                var duplicate = (await _unitOfWork
                    .Repository<AttendanceStatus>()
                    .GetAllAsync())
                    .FirstOrDefault(x =>
                        x.AttendanceStatusId != dto.AttendanceStatusId &&
                        x.AttendanceStatusName.ToLower() == name &&
                        x.CompanyId == dto.CompanyId &&
                        x.RegionId == dto.RegionId &&
                        !x.IsDeleted);

                if (duplicate != null)
                    return new ApiResponse<AttendanceStatusDto>(
                        null, "Attendance status already exists", true);

                entity.AttendanceStatusName = dto.AttendanceStatusName.Trim();
                entity.Description = dto.Description;
                entity.IsActive = dto.IsActive;
                entity.ModifiedBy = dto.ModifiedBy;
                entity.ModifiedAt = DateTime.Now;

                _unitOfWork.Repository<AttendanceStatus>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<AttendanceStatusDto>(
                    MapToDto(entity),
                    "Attendance status updated successfully",
                    true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<AttendanceStatusDto>(
                    null, ex.Message, false);
            }
        }
        #endregion


        #region Soft Delete
        public async Task<ApiResponse<bool>>
            DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork
                    .Repository<AttendanceStatus>()
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<bool>(
                        false, "Record not found", false);

                entity.IsDeleted = true;
                //entity.ModifiedBy = userId;
                entity.ModifiedAt = DateTime.Now;

                _unitOfWork.Repository<AttendanceStatus>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<bool>(
                    true, "Deleted successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(
                    false, ex.Message, false);
            }
        }
        #endregion


        private AttendanceStatusDto MapToDto(AttendanceStatus entity)
        {
            return new AttendanceStatusDto
            {
                AttendanceStatusId = entity.AttendanceStatusId,
                AttendanceStatusName = entity.AttendanceStatusName,
                CompanyId = entity.CompanyId,
                RegionId = entity.RegionId,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }
    }
}
