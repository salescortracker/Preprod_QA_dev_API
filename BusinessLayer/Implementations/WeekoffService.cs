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
    public class WeekoffService: IWeekoffService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeekoffService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET ALL
        public async Task<ApiResponse<IEnumerable<WeekoffDto>>> GetAll(int userId)
        {
            var list = (await _unitOfWork.Repository<Weekoff>()
                .FindAsync(x => !x.IsDeleted && (x.UserId == userId || x.UserId == null)))
                .OrderByDescending(x => x.WeekoffId)
                .ToList();

            var dto = list.Select(x => new WeekoffDto
            {
                WeekoffID = x.WeekoffId,
                CompanyID = x.CompanyId,
                RegionID = x.RegionId,
                WeekoffDate = DateOnly.Parse(x.WeekoffDate!),
                IsActive = x.IsActive
            });

            return new ApiResponse<IEnumerable<WeekoffDto>>(dto, "Weekoff retrieved successfully.");
        }

        //GET BY ID
        public async Task<ApiResponse<WeekoffDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Weekoff>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<WeekoffDto?>(null, "Weekoff not found.", false);

            var dto = new WeekoffDto
            {
                WeekoffID = entity.WeekoffId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                WeekoffDate = DateOnly.Parse(entity.WeekoffDate!),
               IsActive = entity.IsActive
            };

            return new ApiResponse<WeekoffDto?>(dto, "Weekoff retrieved successfully.");
        }

        // CREATE
        public async Task<ApiResponse<string>> CreateAsync(WeekoffDto dto)
        {
            var duplicate = (await _unitOfWork.Repository<Weekoff>().FindAsync(x =>
                !x.IsDeleted &&
                x.CompanyId == dto.CompanyID &&
                x.RegionId == dto.RegionID &&
                x.WeekoffDate == dto.WeekoffDate.ToString("yyyy-MM-dd")))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate Weekoff exists.", false);

            var entity = new Weekoff
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                WeekoffDate = dto.WeekoffDate.ToString("yyyy-MM-dd"),
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = dto.UserId ?? 0,
                UserId = dto.UserId
            };

            await _unitOfWork.Repository<Weekoff>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Weekoff created successfully.");
        }

        // UPDATE
        public async Task<ApiResponse<string>> UpdateAsync(WeekoffDto dto)
        {
            var entity = await _unitOfWork.Repository<Weekoff>()
                .GetByIdAsync(dto.WeekoffID);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Weekoff not found.", false);

            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;
            entity.WeekoffDate   = dto.WeekoffDate.ToString("yyyy-MM-dd");
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<Weekoff>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Weekoff updated successfully.");
        }

        // DELETE (SOFT)
        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Weekoff>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Weekoff not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Weekoff>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Weekoff deleted successfully.");
        }
    }
}
