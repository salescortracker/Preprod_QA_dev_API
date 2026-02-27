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
    public class HelpdeskCategoryAdminService: IHelpdeskCategoryAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HelpdeskCategoryAdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ===============================
        // GET ALL
        // ===============================
        public async Task<ApiResponse<IEnumerable<CreateUpdateHelpdeskCategoryDto>>> GetAll(int userId)
        {
            var list = await _unitOfWork.Repository<HelpDeskCategory>()
                .FindAsync(x =>
                    !x.IsDeleted &&
                    x.UserId == userId);

            var result = list
                .OrderByDescending(x => x.HelpDeskCategoryId)
                .Select(x => new CreateUpdateHelpdeskCategoryDto
                {
                    HelpdeskCategoryID = x.HelpDeskCategoryId,
                    CompanyID = x.CompanyId,
                    RegionID = x.RegionId,
                    CategoryName = x.HelpDeskCategoryName,
                    IsActive = x.IsActive,

                });

            return new ApiResponse<IEnumerable<CreateUpdateHelpdeskCategoryDto>>(
                result,
                "Helpdesk Categories retrieved successfully."
            );
        }

        // ===============================
        // GET BY ID
        // ===============================
        public async Task<ApiResponse<CreateUpdateHelpdeskCategoryDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<HelpDeskCategory>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<CreateUpdateHelpdeskCategoryDto?>(null, "Category not found.", false);

            var dto = new CreateUpdateHelpdeskCategoryDto
            {
                HelpdeskCategoryID = entity.HelpDeskCategoryId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                CategoryName = entity.HelpDeskCategoryName,
                IsActive = entity.IsActive
            };

            return new ApiResponse<CreateUpdateHelpdeskCategoryDto?>(dto, "Category retrieved successfully.");
        }

        // ===============================
        // CREATE
        // ===============================
        public async Task<ApiResponse<string>> CreateAsync(CreateUpdateHelpdeskCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                return new ApiResponse<string>(null!, "Category Name is required.", false);

            var duplicate = (await _unitOfWork.Repository<HelpDeskCategory>().FindAsync(x =>
                !x.IsDeleted &&
                x.CompanyId == dto.CompanyID &&
                x.RegionId == dto.RegionID &&
                x.HelpDeskCategoryName.ToLower() == dto.CategoryName.ToLower()))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate Category exists.", false);

            var entity = new HelpDeskCategory
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                HelpDeskCategoryName = dto.CategoryName,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId,
                UserId = dto.UserId
            };

            await _unitOfWork.Repository<HelpDeskCategory>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Helpdesk Category created successfully.");
        }

        // ===============================
        // UPDATE
        // ===============================
        public async Task<ApiResponse<string>> UpdateAsync(CreateUpdateHelpdeskCategoryDto dto)
        {
            var entity = await _unitOfWork.Repository<HelpDeskCategory>()
                .GetByIdAsync(dto.HelpdeskCategoryID);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Category not found.", false);

            var duplicate = (await _unitOfWork.Repository<HelpDeskCategory>().FindAsync(x =>
                !x.IsDeleted &&
                x.HelpDeskCategoryId != dto.HelpdeskCategoryID &&
                x.CompanyId == dto.CompanyID &&
                x.RegionId == dto.RegionID &&
                x.HelpDeskCategoryName.ToLower() == dto.CategoryName.ToLower()))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate Category exists.", false);
            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;

            entity.HelpDeskCategoryName = dto.CategoryName;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<HelpDeskCategory>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Helpdesk Category updated successfully.");
        }

        // ===============================
        // DELETE (SOFT DELETE)
        // ===============================
        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<HelpDeskCategory>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Category not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<HelpDeskCategory>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Helpdesk Category deleted successfully.");
        }
    }
}
