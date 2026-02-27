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
    public class PolicyCategoryService: IPolicyCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PolicyCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<CreateUpdatePolicyCategoryDto>>> GetAll(int userId)
        {
            var list = (await _unitOfWork.Repository<PolicyCategory>()
                .FindAsync(x => !x.IsDeleted && x.UserId == userId))
                .OrderByDescending(x => x.PolicyCategoryId)
                .ToList();

            var dto = list.Select(x => new CreateUpdatePolicyCategoryDto
            {
                PolicyCategoryId = x.PolicyCategoryId,
                CompanyId = x.CompanyId,
                RegionId = x.RegionId,
                PolicyCategoryName = x.PolicyCategoryName,
                Description = x.Description,
                IsActive = x.IsActive,
            });

            return new ApiResponse<IEnumerable<CreateUpdatePolicyCategoryDto>>(dto, "Policy categories retrieved successfully.");
        }

        public async Task<ApiResponse<CreateUpdatePolicyCategoryDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<PolicyCategory>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<CreateUpdatePolicyCategoryDto?>(null, "Not found.", false);

            var dto = new CreateUpdatePolicyCategoryDto
            {
                PolicyCategoryId = entity.PolicyCategoryId,
                CompanyId = entity.CompanyId,
                RegionId = entity.RegionId,
                PolicyCategoryName = entity.PolicyCategoryName,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return new ApiResponse<CreateUpdatePolicyCategoryDto?>(dto, "Record retrieved successfully.");
        }

        public async Task<ApiResponse<string>> CreateAsync(CreateUpdatePolicyCategoryDto dto)
        {
            var duplicate = (await _unitOfWork.Repository<PolicyCategory>()
                .FindAsync(x =>
                    !x.IsDeleted &&
                    x.CompanyId == dto.CompanyId &&
                    x.RegionId == dto.RegionId &&
                    x.PolicyCategoryName.ToLower() == dto.PolicyCategoryName.ToLower()))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate Policy Category exists.", false);

            var entity = new PolicyCategory
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                PolicyCategoryName = dto.PolicyCategoryName,
                Description = dto.Description,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId ?? 0,
                UserId = dto.UserId
            };

            await _unitOfWork.Repository<PolicyCategory>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Policy Category created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateAsync(CreateUpdatePolicyCategoryDto dto)
        {
            var entity = await _unitOfWork.Repository<PolicyCategory>().GetByIdAsync(dto.PolicyCategoryId);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Not found.", false);

            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.PolicyCategoryName = dto.PolicyCategoryName;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<PolicyCategory>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Policy Category updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<PolicyCategory>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<PolicyCategory>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Deleted successfully.");
        }

    }
}
