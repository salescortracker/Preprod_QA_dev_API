using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class RecruitmentNoticePeriodService : IRecruitmentNoticePeriodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecruitmentNoticePeriodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ================= GET ALL =================
        public async Task<ApiResponse<IEnumerable<RecruitmentNoticePeriodDto>>> GetAllRecruitmentNoticePeriodService(int userId)
        {
            var list = (await _unitOfWork.Repository<RecruitmentNoticePeriod>()
                .FindAsync(x => !x.IsDeleted && x.UserId == userId))
                .OrderByDescending(x => x.RecruitmentNoticePeriodId)
                .ToList();

            var dto = list.Select(x => new RecruitmentNoticePeriodDto
            {
                RecruitmentNoticePeriodID = x.RecruitmentNoticePeriodId,
                CompanyID = x.CompanyId,
                RegionID = x.RegionId,
                NoticePeriod = x.NoticePeriod,
                IsActive = x.IsActive
            });

            return new ApiResponse<IEnumerable<RecruitmentNoticePeriodDto>>(dto,
                "Recruitment Notice Period retrieved successfully.");
        }

        // ================= GET BY ID =================
        public async Task<ApiResponse<RecruitmentNoticePeriodDto?>> GetByIdRecruitmentNoticePeriodServiceAsync(int id)
        {
            var entity = await _unitOfWork.Repository<RecruitmentNoticePeriod>()
                .GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<RecruitmentNoticePeriodDto?>(null,
                    "Recruitment Notice Period not found.", false);

            var dto = new RecruitmentNoticePeriodDto
            {
                RecruitmentNoticePeriodID = entity.RecruitmentNoticePeriodId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                NoticePeriod = entity.NoticePeriod,
                IsActive = entity.IsActive
            };

            return new ApiResponse<RecruitmentNoticePeriodDto?>(dto,
                "Recruitment Notice Period retrieved successfully.");
        }

        // ================= CREATE =================
        public async Task<ApiResponse<string>> CreateRecruitmentNoticePeriodServiceAsync(RecruitmentNoticePeriodDto dto)
        {
            var duplicate = (await _unitOfWork.Repository<RecruitmentNoticePeriod>()
                .FindAsync(x =>
                    !x.IsDeleted &&
                    x.CompanyId == dto.CompanyID &&
                    x.RegionId == dto.RegionID &&
                    x.NoticePeriod == dto.NoticePeriod))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!,
                    "Duplicate Notice Period exists.", false);

            var entity = new RecruitmentNoticePeriod
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                NoticePeriod = dto.NoticePeriod,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId ?? 0,
                UserId = dto.UserId
            };

            await _unitOfWork.Repository<RecruitmentNoticePeriod>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Recruitment Notice Period created successfully.");
        }

        // ================= UPDATE =================
        public async Task<ApiResponse<string>> UpdateRecruitmentNoticePeriodServiceAsync(RecruitmentNoticePeriodDto dto)
        {
            var entity = await _unitOfWork.Repository<RecruitmentNoticePeriod>()
                .GetByIdAsync(dto.RecruitmentNoticePeriodID);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!,
                    "Recruitment Notice Period not found.", false);

            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;
            entity.NoticePeriod = dto.NoticePeriod;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<RecruitmentNoticePeriod>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Recruitment Notice Period updated successfully.");
        }

        // ================= DELETE =================
        public async Task<ApiResponse<string>> DeleteRecruitmentNoticePeriodServiceAsync(int id)
        {
            var entity = await _unitOfWork.Repository<RecruitmentNoticePeriod>()
                .GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!,
                    "Recruitment Notice Period not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<RecruitmentNoticePeriod>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Recruitment Notice Period deleted successfully.");
        }
    }
}
