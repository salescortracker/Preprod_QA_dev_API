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
    public class PriorityService: IPriorityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriorityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =====================================
        // GET ALL
        // =====================================
        public async Task<ApiResponse<IEnumerable<PriorityDto>>> GetAll(int userId)
        {
            try
            {
                var list = (await _unitOfWork.Repository<Priority>()
                    .FindAsync(x => !x.IsDeleted && x.UserId == userId))
                    .OrderByDescending(x => x.PriorityId)
                    .Select(x => new PriorityDto
                    {
                        PriorityId = x.PriorityId,
                        CompanyId = x.CompanyId,
                        RegionId = x.RegionId,
                        PriorityName = x.PriorityName,
                        Description = x.Description,
                        IsActive = x.IsActive,
                        UserId = x.UserId
                    });

                return new ApiResponse<IEnumerable<PriorityDto>>(list, "Priorities retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PriorityDto>>(null!,
                    $"Failed to retrieve priorities. {ex.Message}", false);
            }
        }

        // =====================================
        // GET BY ID
        // =====================================
        public async Task<ApiResponse<PriorityDto?>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Priority>().GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<PriorityDto?>(null, "Priority not found.", false);

                var dto = new PriorityDto
                {
                    PriorityId = entity.PriorityId,
                    CompanyId = entity.CompanyId,
                    RegionId = entity.RegionId,
                    PriorityName = entity.PriorityName,
                    Description = entity.Description,
                    IsActive = entity.IsActive,
                    UserId = entity.UserId
                };

                return new ApiResponse<PriorityDto?>(dto, "Priority retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<PriorityDto?>(null,
                    $"Failed to retrieve priority. {ex.Message}", false);
            }
        }

        // =====================================
        // CREATE
        // =====================================
        public async Task<ApiResponse<string>> CreateAsync(PriorityDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.PriorityName))
                    return new ApiResponse<string>(null!, "Priority Name is required.", false);

                // Duplicate check
                var duplicate = (await _unitOfWork.Repository<Priority>().FindAsync(x =>
                    !x.IsDeleted &&
                    x.CompanyId == dto.CompanyId &&
                    x.RegionId == dto.RegionId &&
                    x.PriorityName.ToLower() == dto.PriorityName.ToLower()))
                    .Any();

                if (duplicate)
                    return new ApiResponse<string>(null!, "Duplicate Priority exists.", false);

                var entity = new Priority
                {
                    CompanyId = dto.CompanyId,
                    RegionId = dto.RegionId,
                    PriorityName = dto.PriorityName,
                    Description = dto.Description,
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId,
                    UserId = dto.UserId
                };

                await _unitOfWork.Repository<Priority>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<string>("Priority created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null!,
                    $"Create failed. {ex.Message}", false);
            }
        }

        // =====================================
        // UPDATE
        // =====================================
        public async Task<ApiResponse<string>> UpdateAsync(PriorityDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Priority>().GetByIdAsync(dto.PriorityId);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<string>(null!, "Priority not found.", false);

                // Duplicate check
                var duplicate = (await _unitOfWork.Repository<Priority>().FindAsync(x =>
                    !x.IsDeleted &&
                    x.PriorityId != dto.PriorityId &&
                    x.CompanyId == dto.CompanyId &&
                    x.RegionId == dto.RegionId &&
                    x.PriorityName.ToLower() == dto.PriorityName.ToLower()))
                    .Any();

                if (duplicate)
                    return new ApiResponse<string>(null!, "Duplicate Priority exists.", false);

                entity.PriorityName = dto.PriorityName;
                entity.Description = dto.Description;
                entity.IsActive = dto.IsActive;
                entity.ModifiedAt = DateTime.UtcNow;
                entity.ModifiedBy = dto.UserId;

                _unitOfWork.Repository<Priority>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<string>("Priority updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(null!,
                    $"Update failed. {ex.Message}", false);
            }
        }

        // =====================================
        // DELETE (SOFT DELETE)
        // =====================================
        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Priority>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Priority not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = entity.UserId;

            _unitOfWork.Repository<Priority>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Priority deleted successfully.");
        }
    }
}
