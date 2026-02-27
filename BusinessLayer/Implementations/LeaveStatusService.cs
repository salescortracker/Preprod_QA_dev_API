using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class LeaveStatusService: ILeaveStatusService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LeaveStatusService(IUnitOfWork unitOfWork, HRMSContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #region Get All
        public async Task<ApiResponse<IEnumerable<LeaveStatusDto>>>
      GetAllLeaveStatusAsync(int companyId, int regionId)
        {
            try
            {
                if (companyId <= 0)
                    return new ApiResponse<IEnumerable<LeaveStatusDto>>(
                        null, "Invalid company id", false);

                var data = await _unitOfWork
                    .Repository<LeaveStatus>()
                    .GetAllAsync();

                var result = data
                    .Where(x =>
                        x.CompanyId == companyId &&
                        x.RegionId == regionId &&
                        !x.IsDeleted)
                    .Select(MapToDto)
                    .ToList();

                return new ApiResponse<IEnumerable<LeaveStatusDto>>(
                    result,
                    "Leave statuses fetched successfully",
                    true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<LeaveStatusDto>>(
                    null,
                    ex.Message,
                    false);
            }
        }

        #endregion


        #region Get By Id
        public async Task<ApiResponse<LeaveStatusDto?>>
            GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork
                    .Repository<LeaveStatus>()
                    .GetByIdAsync(id);

                if (entity == null)
                    return new ApiResponse<LeaveStatusDto?>(
                        null,
                        "Leave status not found",
                        false
                    );

                return new ApiResponse<LeaveStatusDto?>(
                    MapToDto(entity),
                    "Leave status fetched successfully",
                    true
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<LeaveStatusDto?>(
                    null,
                    ex.Message,
                    false
                );
            }
        }
        #endregion


        #region Create
        public async Task<ApiResponse<LeaveStatusDto>>
      CreateAsync(LeaveStatusDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.LeaveStatusName))
                    return new ApiResponse<LeaveStatusDto>(
                        null, "Leave status name is required", false);

                var name = dto.LeaveStatusName.Trim().ToLower();

                var exists = (await _unitOfWork
                    .Repository<LeaveStatus>()
                    .GetAllAsync())
                    .Any(x =>
                        x.LeaveStatusName.ToLower() == name &&
                        x.CompanyId == dto.CompanyID &&
                        x.RegionId == dto.RegionID &&
                        !x.IsDeleted);

                if (exists)
                    return new ApiResponse<LeaveStatusDto>(
                        null,
                        "Leave status already exists",
                        false);

                var entity = new LeaveStatus
                {
                    LeaveStatusName = dto.LeaveStatusName.Trim(),
                    CompanyId = dto.CompanyID,
                    RegionId = dto.RegionID,
                    Description = dto.Description,
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedBy = dto.UserID,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.Repository<LeaveStatus>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<LeaveStatusDto>(
                    MapToDto(entity),
                    "Leave status created successfully",
                    true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<LeaveStatusDto>(
                    null,
                    ex.Message,
                    false);
            }
        }

        #endregion


        #region Update
        public async Task<ApiResponse<LeaveStatusDto>>
         UpdateAsync(LeaveStatusDto dto)
        {
            try
            {
                var entity = await _unitOfWork
                    .Repository<LeaveStatus>()
                    .GetByIdAsync(dto.LeaveStatusID);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<LeaveStatusDto>(
                        null,
                        "Leave status not found",
                        false);

                var name = dto.LeaveStatusName.Trim().ToLower();

                var duplicate = (await _unitOfWork
                    .Repository<LeaveStatus>()
                    .GetAllAsync())
                    .Any(x =>
                        x.LeaveStatusId != dto.LeaveStatusID &&
                        x.LeaveStatusName.ToLower() == name &&
                        x.CompanyId == dto.CompanyID &&
                        x.RegionId == dto.RegionID &&
                        !x.IsDeleted);

                if (duplicate)
                    return new ApiResponse<LeaveStatusDto>(
                        null,
                        "Leave status already exists",
                        false);

                entity.LeaveStatusName = dto.LeaveStatusName.Trim();
                entity.Description = dto.Description;
                entity.IsActive = dto.IsActive;
                entity.ModifiedBy = dto.UserID;
                entity.ModifiedAt = DateTime.Now;

                _unitOfWork.Repository<LeaveStatus>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<LeaveStatusDto>(
                    MapToDto(entity),
                    "Leave status updated successfully",
                    true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<LeaveStatusDto>(
                    null,
                    ex.Message,
                    false);
            }
        }

        #endregion


        #region Delete
        public async Task<ApiResponse<bool>>
            DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork
                    .Repository<LeaveStatus>()
                    .GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<bool>(
                        false,
                        "Record not found",
                        false
                    );

                entity.IsDeleted = true;
                entity.ModifiedAt = DateTime.Now;

                _unitOfWork.Repository<LeaveStatus>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<bool>(
                    true,
                    "Deleted successfully",
                    true
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(
                    false,
                    ex.Message,
                    false
                );
            }
        }
        #endregion


        private LeaveStatusDto MapToDto(LeaveStatus entity)
        {
            return new LeaveStatusDto
            {
                LeaveStatusID = entity.LeaveStatusId,
                LeaveStatusName = entity.LeaveStatusName,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }
    }
}
