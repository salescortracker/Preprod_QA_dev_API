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
    public class HolidayListService: IHolidayListService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HolidayListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET ALL
        public async Task<ApiResponse<IEnumerable<HolidayListDto>>> GetAll(int userId)
        {
            var list = (await _unitOfWork.Repository<HolidayList>()
                .FindAsync(x => !x.IsDeleted && x.UserId == userId))
                .OrderByDescending(x => x.HolidayListId)
                .ToList();

            var dto = list.Select(x => new HolidayListDto
            {
                HolidayListID = x.HolidayListId,
                CompanyID = x.CompanyId,
                RegionID = x.RegionId,
                HolidayListName = x.HolidayListName,
                Date = x.Date,
                IsActive = x.IsActive
            });

            return new ApiResponse<IEnumerable<HolidayListDto>>(dto, "Holiday List retrieved successfully.");
        }

        // GET BY ID
        public async Task<ApiResponse<HolidayListDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<HolidayList>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<HolidayListDto?>(null, "Holiday not found.", false);

            var dto = new HolidayListDto
            {
                HolidayListID = entity.HolidayListId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                HolidayListName = entity.HolidayListName,
                Date = entity.Date,
                IsActive = entity.IsActive
            };

            return new ApiResponse<HolidayListDto?>(dto, "Holiday retrieved successfully.");
        }

        // CREATE
        public async Task<ApiResponse<string>> CreateAsync(CreateUpdateHolidayListDto dto)
        {
            var duplicate = (await _unitOfWork.Repository<HolidayList>().FindAsync(x =>
                !x.IsDeleted &&
                x.CompanyId == dto.CompanyID &&
                x.RegionId == dto.RegionID &&
                x.HolidayListName.ToLower() == dto.HolidayListName.ToLower()))
                .Any();

            if (duplicate)
                return new ApiResponse<string>(null!, "Duplicate Holiday exists.", false);

            var entity = new HolidayList
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                HolidayListName = dto.HolidayListName,
                Date = dto.Date,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UserId = dto.UserId,
                CreatedBy = dto.UserId ?? 0
            };

            await _unitOfWork.Repository<HolidayList>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Holiday created successfully.");
        }

        // UPDATE
        public async Task<ApiResponse<string>> UpdateAsync(CreateUpdateHolidayListDto dto)
        {
            var entity = await _unitOfWork.Repository<HolidayList>()
                .GetByIdAsync(dto.HolidayListID);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Holiday not found.", false);

            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;

            entity.HolidayListName = dto.HolidayListName;
            entity.Date = dto.Date;
            entity.IsActive = dto.IsActive;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedBy = dto.UserId;

            _unitOfWork.Repository<HolidayList>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Holiday updated successfully.");
        }

        // DELETE (SOFT)
        public async Task<ApiResponse<string>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<HolidayList>().GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return new ApiResponse<string>(null!, "Holiday not found.", false);

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<HolidayList>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<string>("Holiday deleted successfully.");
        }
    }
}
