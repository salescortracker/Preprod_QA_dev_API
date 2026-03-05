using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class ScreeningResultService : IScreeningResultService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScreeningResultService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<ScreeningResultDto>>> GetAll(int userId)
        {
            var list = (await _unitOfWork.Repository<ScreeningResult>()
                .FindAsync(x => !x.IsDeleted && x.UserId == userId))
                .OrderByDescending(x => x.ScreeningResultId)
                .ToList();

            var dto = list.Select(x => new ScreeningResultDto
            {
                ScreeningResultID = x.ScreeningResultId,
                CompanyID = x.CompanyId,
                RegionID = x.RegionId,
                Weekoff = x.Weekoff,
                IsActive = x.IsActive
            });

            return new ApiResponse<IEnumerable<ScreeningResultDto>>(dto, "Screening Results retrieved successfully.");
        }

        public async Task<ApiResponse<ScreeningResultDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ScreeningResult>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<ScreeningResultDto?>(null, "Record not found", false);

            var dto = new ScreeningResultDto
            {
                ScreeningResultID = entity.ScreeningResultId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                Weekoff = entity.Weekoff,
                IsActive = entity.IsActive
            };

            return new ApiResponse<ScreeningResultDto?>(dto);
        }

        public async Task<ApiResponse<string>> CreateAsync(ScreeningResultDto dto)
        {
            var entity = new ScreeningResult
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                Weekoff = dto.Weekoff,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId ?? 0,
                UserId = dto.UserId
            };

            await _unitOfWork.Repository<ScreeningResult>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Screening Result created successfully.");
        }

        public async Task<ApiResponse<string>> UpdateAsync(ScreeningResultDto dto)
        {
            var entity = await _unitOfWork.Repository<ScreeningResult>()
                .GetByIdAsync(dto.ScreeningResultID);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Record not found", false);

            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;
            entity.Weekoff = dto.Weekoff;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<ScreeningResult>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Screening Result updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ScreeningResult>()
                .GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Record not found", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ScreeningResult>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Screening Result deleted successfully.");
        }
    }
}
