using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class DesignationService:IDesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HRMSContext _hrmsContext;

        public DesignationService(IUnitOfWork unitOfWork,HRMSContext hRMSContext)
        {
            _unitOfWork = unitOfWork;
            _hrmsContext = hRMSContext;
        }

        public async Task<ApiResponse<IEnumerable<DesignationDTO>>> GetAllAsync(int userId)
        {
            try
            {
                var list = await _unitOfWork.Repository<Designation>().FindAsync(d => !d.IsDeleted);
                var dto = list.Where(x=>x.UserId==userId).Select(d => new DesignationDTO
                {
                    DesignationID = d.DesignationId,
                    CompanyID = d.CompanyId,
                    RegionID = d.RegionId,
                    DesignationName = d.DesignationName,
                    Description = d.Description,
                    IsActive = d.IsActive,
                    companyName=d.CompanyId!=null?_hrmsContext.Companies.Where(x=>x.CompanyId==d.CompanyId).FirstOrDefault().CompanyName:null,
                    regionName=d.RegionId!=null?_hrmsContext.Regions.Where(x=>x.RegionId==d.RegionId).FirstOrDefault().RegionName:null
                });

                return new ApiResponse<IEnumerable<DesignationDTO>>(dto, "Designations retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<DesignationDTO>>(null!, $"Failed to get designations. {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<DesignationDTO?>> GetByIdAsync(int id)
        {
            try
            {
                var d = await _unitOfWork.Repository<Designation>().GetByIdAsync(id);
                if (d == null || d.IsDeleted)
                    return new ApiResponse<DesignationDTO?>(null, "Designation not found.", false);

                var dto = new DesignationDTO
                {
                    DesignationID = d.DesignationId,
                    CompanyID = d.CompanyId,
                    RegionID = d.RegionId,
                    DesignationName = d.DesignationName,
                    Description = d.Description,
                    IsActive = d.IsActive
                };
                return new ApiResponse<DesignationDTO?>(dto, "Designation retrieved.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<DesignationDTO?>(null, $"Failed to get designation. {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<DesignationDTO>> CreateAsync(CreateUpdateDesignationDto dto)
        {
            try
            {
                var exists = (await _unitOfWork.Repository<Designation>().FindAsync(d =>
                    !d.IsDeleted &&
                    d.CompanyId == dto.CompanyID &&
                    d.RegionId == dto.RegionID &&
                    d.DesignationName.ToLower() == dto.DesignationName.ToLower()))
                    .Any();

                if (exists)
                    return new ApiResponse<DesignationDTO>(null!, "Duplicate designation exists.", false);

                var entity = new Designation
                {
                    CompanyId = dto.CompanyID,
                    RegionId = dto.RegionID,
                    DesignationName = dto.DesignationName,
                    Description = dto.Description,
                    IsActive = dto.IsActive,   
                    CreatedBy=dto.createdBy,   
                    CreatedAt = DateTime.UtcNow,
                    UserId=dto.userId
                };

                await _unitOfWork.Repository<Designation>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                var resultDto = new DesignationDTO
                {
                    DesignationID = entity.DesignationId,
                    CompanyID = entity.CompanyId,
                    RegionID = entity.RegionId,
                    DesignationName = entity.DesignationName,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                return new ApiResponse<DesignationDTO>(resultDto, "Designation created successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<DesignationDTO>(null!, $"Create failed. {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<DesignationDTO>> UpdateAsync(int id,CreateUpdateDesignationDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Designation>().GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<DesignationDTO>(null!, "Designation not found.", false);

                var dup = (await _unitOfWork.Repository<Designation>().FindAsync(d =>
                    !d.IsDeleted &&
                    d.DesignationId != id &&
                    d.CompanyId == dto.CompanyID &&
                    d.RegionId == dto.RegionID &&
                    d.DesignationName.ToLower() == dto.DesignationName.ToLower())).Any();

                if (dup)
                    return new ApiResponse<DesignationDTO>(null!, "Duplicate designation exists.", false);

                entity.CompanyId = dto.CompanyID;
                entity.RegionId = dto.RegionID;
                entity.DesignationName = dto.DesignationName;
                entity.Description = dto.Description;
                entity.IsActive = dto.IsActive;
                entity.ModifiedBy = dto.modifiedBy;
                entity.ModifiedAt = DateTime.UtcNow;

                _unitOfWork.Repository<Designation>().Update(entity);
                await _unitOfWork.CompleteAsync();

                var resDto = new DesignationDTO
                {
                    DesignationID = entity.DesignationId,
                    CompanyID = entity.CompanyId,
                    RegionID = entity.RegionId,
                    DesignationName = entity.DesignationName,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                return new ApiResponse<DesignationDTO>(resDto, "Designation updated successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<DesignationDTO>(null!, $"Update failed. {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<object>> SoftDeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Designation>().GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                    return new ApiResponse<object>(null!, "Designation not found.", false);

                entity.IsDeleted = true;
                entity.ModifiedBy = entity.ModifiedBy;
                entity.ModifiedAt = DateTime.UtcNow;

                _unitOfWork.Repository<Designation>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<object>(null!, "Designation deleted successfully (soft delete).");
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>(null!, $"Delete failed. {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<(int inserted, int duplicates, int failed)>> BulkInsertAsync(IEnumerable<CreateUpdateDesignationDto> items, int createdBy)
        {
            int inserted = 0, duplicates = 0, failed = 0;
            try
            {
                foreach (var dto in items)
                {
                    try
                    {
                        var exists = (await _unitOfWork.Repository<Designation>().FindAsync(d =>
                            !d.IsDeleted &&
                            d.CompanyId == dto.CompanyID &&
                            d.RegionId == dto.RegionID &&
                            d.DesignationName.ToLower() == dto.DesignationName.ToLower()))
                            .Any();

                        if (exists)
                        {
                            duplicates++;
                            continue;
                        }

                        var entity = new Designation
                        {
                            CompanyId = dto.CompanyID,
                            RegionId = dto.RegionID,
                            DesignationName = dto.DesignationName,
                            Description = dto.Description,
                            IsActive = dto.IsActive,
                            CreatedBy = createdBy,
                            CreatedAt = DateTime.UtcNow
                        };

                        await _unitOfWork.Repository<Designation>().AddAsync(entity);
                        inserted++;
                    }
                    catch
                    {
                        failed++;
                    }
                }

                await _unitOfWork.CompleteAsync();
                return new ApiResponse<(int, int, int)>((inserted, duplicates, failed),
                    $"{inserted} inserted, {duplicates} duplicates, {failed} failed");
            }
            catch (Exception ex)
            {
                return new ApiResponse<(int, int, int)>((inserted, duplicates, failed),
                    $"Bulk insert failed. {ex.Message}", false);
            }
        }

    }
}
