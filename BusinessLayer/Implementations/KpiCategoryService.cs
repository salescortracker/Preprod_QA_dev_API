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
    public class KpiCategoryService: IKpiCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KpiCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -----------------------------
        // GET ALL
        // -----------------------------
        public async Task<ApiResponse<IEnumerable<KpiCategoryDto>>> GetAll()
        {
            try
            {
                var list = (await _unitOfWork.Repository<KpiCategory>()
                    .FindAsync(x => !x.IsDeleted))
                    .ToList();

                var dto = list
                    .OrderByDescending(x => x.KpiCategoryId)
                    .Select(x => new KpiCategoryDto
                    {
                        KpiCategoryID = x.KpiCategoryId,
                        CompanyID = x.CompanyId,
                        RegionID = x.RegionId,
                        KpiCategoryName = x.KpiCategoryName ?? string.Empty,
                       // Description = x.Description,
                        IsActive = x.IsActive
                    });

                return new ApiResponse<IEnumerable<KpiCategoryDto>>(
                    dto,
                    "KPI Categories retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResponse<IEnumerable<KpiCategoryDto>>(
                    null!,
                    $"Failed to retrieve KPI Categories. {ex.Message}",
                    false
                );
            }
        }

        // -----------------------------
        // GET BY ID
        // -----------------------------
        public async Task<ApiResponse<KpiCategoryDto?>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Repository<KpiCategory>().GetByIdAsync(id);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<KpiCategoryDto?>(null, "KPI Category not found.", false);

                var dto = new KpiCategoryDto
                {
                    KpiCategoryID = entity.KpiCategoryId,
                    CompanyID = entity.CompanyId,
                    RegionID = entity.RegionId,
                    KpiCategoryName = entity.KpiCategoryName ?? string.Empty,
                   // Description = entity.Description,
                    IsActive = entity.IsActive
                };

                return new ApiResponse<KpiCategoryDto?>(dto, "KPI Category retrieved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ApiResponse<KpiCategoryDto?>(null, $"Failed to retrieve KPI Category. {ex.Message}", false);
            }
        }

        // -----------------------------
        // CREATE
        // -----------------------------
        public async Task<ApiResponse<string>> CreateAsync(CreateUpdateKpiCategoryDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.KpiCategoryName))
                    return new ApiResponse<string>(null!, "KPI Category Name is required.", false);

                // Duplicate check
                var duplicate = (await _unitOfWork.Repository<KpiCategory>().FindAsync(x =>
                    !x.IsDeleted &&
                    x.CompanyId == dto.CompanyID &&
                    x.RegionId == dto.RegionID &&
                    x.KpiCategoryName != null &&
                    x.KpiCategoryName.ToLower() == dto.KpiCategoryName.ToLower()))
                    .Any();

                if (duplicate)
                    return new ApiResponse<string>(null!, "Duplicate KPI Category exists.", false);

                var entity = new KpiCategory
                {
                    CompanyId = dto.CompanyID,
                    RegionId = dto.RegionID,
                    KpiCategoryName = dto.KpiCategoryName,
                   // Description = dto.Description,
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<KpiCategory>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<string>("KPI Category created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ApiResponse<string>(null!, $"Create failed. {ex.Message}", false);
            }
        }

        // -----------------------------
        // UPDATE
        // -----------------------------
        public async Task<ApiResponse<string>> UpdateAsync(CreateUpdateKpiCategoryDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Repository<KpiCategory>().GetByIdAsync(dto.KpiCategoryID);

                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<string>(null!, "KPI Category not found.", false);

                // Duplicate check
                var duplicate = (await _unitOfWork.Repository<KpiCategory>().FindAsync(x =>
                    !x.IsDeleted &&
                    x.KpiCategoryId != dto.KpiCategoryID &&
                    x.CompanyId == dto.CompanyID &&
                    x.RegionId == dto.RegionID &&
                    x.KpiCategoryName != null &&
                    x.KpiCategoryName.ToLower() == dto.KpiCategoryName.ToLower()))
                    .Any();

                if (duplicate)
                    return new ApiResponse<string>(null!, "Duplicate KPI Category exists.", false);

                entity.KpiCategoryName = dto.KpiCategoryName;
               // entity.Description = dto.Description;
                entity.IsActive = dto.IsActive;
                entity.ModifiedAt = DateTime.UtcNow;

                _unitOfWork.Repository<KpiCategory>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<string>("KPI Category updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ApiResponse<string>(null!, $"Update failed. {ex.Message}", false);
            }
        }

        // ---------------------------
        // DELETE (HARD DELETE)
        // ---------------------------
        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<KpiCategory>().GetByIdAsync(id);

            if (entity == null)
                return new ApiResponse<string>(null!, "KPI Category not found.", false);

            _unitOfWork.Repository<KpiCategory>().Remove(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("KPI Category deleted permanently.");
        }
    }
}
