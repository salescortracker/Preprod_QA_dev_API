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
    public class ProjectStatusAdminService: IProjectStatusAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectStatusAdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<IEnumerable<ProjectStatusDto>>> GetAllProject(int userId)
        {
            var list = await _unitOfWork.Repository<ProjectStatus>()
                .FindAsync(x =>
                    !x.IsDeleted &&
                    x.UserId == userId);

            var result = list
                .OrderByDescending(x => x.ProjectStatusId)
                .Select(x => new ProjectStatusDto
                {
                    ProjectStatusId = x.ProjectStatusId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    ProjectStatusName = x.ProjectStatusName,
                    IsActive = x.IsActive,

                });

            return new ApiResponse<IEnumerable<ProjectStatusDto>>(
                result,
                "Helpdesk Categories retrieved successfully."
            );
        }


        // ===============================
        // GET BY ID
        // ===============================
        public async Task<ApiResponse<ProjectStatus?>> GetByIdProjectAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ProjectStatus>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<ProjectStatus?>(null, "status not found.", false);

            var dto = new ProjectStatus
            {
                ProjectStatusId = entity.ProjectStatusId,
                CompanyId = entity.CompanyId,
                RegionId = entity.RegionId,
                ProjectStatusName = entity.ProjectStatusName,
                IsActive = entity.IsActive,
            };

            return new ApiResponse<ProjectStatus?>(dto, "Category retrieved successfully.");
        }

        // ===============================
        // CREATE
        // ===============================
        public async Task<ApiResponse<string>> CreateProjectAsync(ProjectStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProjectStatusName))
                return new ApiResponse<string>(null!, "Staus Name is required.", false);

            var duplicate = (await _unitOfWork.Repository<ProjectStatus>().FindAsync(x =>
                !x.IsDeleted &&
                x.CompanyId == dto.CompanyId &&
                x.RegionId == dto.RegionId &&
                x.ProjectStatusName.ToLower() == dto.ProjectStatusName.ToLower()))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate status exists.", false);

            var entity = new ProjectStatus
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                ProjectStatusName = dto.ProjectStatusName,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,

                UserId = dto.UserId
            };

            await _unitOfWork.Repository<ProjectStatus>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Project status created successfully.");
        }

        // ===============================
        // UPDATE
        // ===============================
        public async Task<ApiResponse<string>> UpdateProjectAsync(ProjectStatusDto dto)
        {
            var entity = await _unitOfWork.Repository<ProjectStatus>()
                .GetByIdAsync(dto.ProjectStatusId);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "status not found.", false);

            var duplicate = (await _unitOfWork.Repository<ProjectStatus>().FindAsync(x =>
                !x.IsDeleted &&
                x.ProjectStatusId != dto.ProjectStatusId &&
                x.CompanyId == dto.CompanyId &&
                x.RegionId == dto.RegionId &&
                x.ProjectStatusName.ToLower() == dto.ProjectStatusName.ToLower()))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate status exists.", false);
            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;

            entity.ProjectStatusName = dto.ProjectStatusName;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<ProjectStatus>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Project status updated successfully.");
        }

        // ===============================
        // DELETE (SOFT DELETE)
        // ===============================
        public async Task<ApiResponse<string>> DeleteProjectAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ProjectStatus>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "status not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ProjectStatus>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Project status deleted successfully.");
        }
    }
}
