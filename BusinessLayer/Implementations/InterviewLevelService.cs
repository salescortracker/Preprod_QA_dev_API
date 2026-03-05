using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class InterviewLevelService : IInterviewLevelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewLevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<InterviewLevelDto>>> GetAll(int userId)
        {
            var list = (await _unitOfWork.Repository<InterviewLevel>()
                .FindAsync(x => !x.IsDeleted && x.UserId == userId))
                .OrderByDescending(x => x.InterviewLevelsId)
                .ToList();

            var dto = list.Select(x => new InterviewLevelDto
            {
                InterviewLevelsID = x.InterviewLevelsId,
                CompanyID = x.CompanyId,
                RegionID = x.RegionId,
                InterviewLevels = x.InterviewLevels,
                IsActive = x.IsActive
            });

            return new ApiResponse<IEnumerable<InterviewLevelDto>>(dto, "Interview Levels retrieved successfully.");
        }

        public async Task<ApiResponse<string>> CreateAsync(InterviewLevelDto dto)
        {
            var entity = new InterviewLevel
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                InterviewLevels = dto.InterviewLevels,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId ?? 0,
                UserId = dto.UserId
            };

            await _unitOfWork.Repository<InterviewLevel>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Interview Level created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateAsync(InterviewLevelDto dto)
        {
            var entity = await _unitOfWork.Repository<InterviewLevel>()
                .GetByIdAsync(dto.InterviewLevelsID);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Record not found", false);

            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;
            entity.InterviewLevels = dto.InterviewLevels;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<InterviewLevel>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Interview Level updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<InterviewLevel>()
                .GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Record not found", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<InterviewLevel>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Interview Level deleted successfully.");
        }

        public async Task<ApiResponse<InterviewLevelDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<InterviewLevel>()
                .GetByIdAsync(id);

            if (entity == null)
                return new ApiResponse<InterviewLevelDto?>(null, "Record not found", false);

            var dto = new InterviewLevelDto
            {
                InterviewLevelsID = entity.InterviewLevelsId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                InterviewLevels = entity.InterviewLevels,
                IsActive = entity.IsActive
            };

            return new ApiResponse<InterviewLevelDto?>(dto);
        }
    }
}
