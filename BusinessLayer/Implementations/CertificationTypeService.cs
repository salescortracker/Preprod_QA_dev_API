using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class CertificationTypeService: ICertificationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CertificationTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ================= GET ALL =================
        public async Task<ApiResponse<IEnumerable<CertificationTypeDto>>> GetAllAsync(
            int userId, int regionId)
        {
            var list = await _unitOfWork.Repository<CertificationType>()
                .FindAsync(x =>
                    x.UserId == userId );

            return new ApiResponse<IEnumerable<CertificationTypeDto>>(
                list.Select(x => new CertificationTypeDto
                {
                    CertificationTypeID = x.CertificationTypeId,
                    CompanyID = x.CompanyId,
                    RegionID = x.RegionId,
                    CertificationTypeName = x.CertificationTypeName,
                    IsActive = x.IsActive ?? false
                })
            );
        }

        public async Task<ApiResponse<IEnumerable<CertificationTypeDto>>> GetCmpregionAllAsync(
          int companyId, int regionId)
        {
            var list = await _unitOfWork.Repository<CertificationType>()
                .FindAsync(x =>
                    x.CompanyId == companyId && x.RegionId==regionId);

            return new ApiResponse<IEnumerable<CertificationTypeDto>>(
                list.Select(x => new CertificationTypeDto
                {
                    CertificationTypeID = x.CertificationTypeId,
                    CompanyID = x.CompanyId,
                    RegionID = x.RegionId,
                    CertificationTypeName = x.CertificationTypeName,
                    IsActive = x.IsActive ?? false
                })
            );
        }


        // ================= GET BY ID =================
        public async Task<ApiResponse<CertificationTypeDto?>> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork
                .Repository<CertificationType>()
                .GetByIdAsync(id);

            if (entity == null)
                return new ApiResponse<CertificationTypeDto?>(
                    null, "Certification type not found.", false);

            return new ApiResponse<CertificationTypeDto?>(
                new CertificationTypeDto
                {
                    CertificationTypeID = entity.CertificationTypeId,
                    CompanyID = entity.CompanyId,
                    RegionID = entity.RegionId,
                    CertificationTypeName = entity.CertificationTypeName,
                    IsActive = entity.IsActive ?? false
                });
        }

        // ================= CREATE =================
        public async Task<ApiResponse<CertificationTypeDto>> CreateAsync(
        CreateUpdateCertificationTypeDto dto)
        {
            var exists = (await _unitOfWork.Repository<CertificationType>()
                .FindAsync(x =>
                    x.CertificationTypeName.ToLower() ==
                    dto.CertificationTypeName.ToLower()))
                .Any();

            if (exists)
                return new ApiResponse<CertificationTypeDto>(
                    null!, "Certification Type already exists.", false);

            var entity = new CertificationType
            {
                CompanyId = dto.CompanyID,
                RegionId = dto.RegionID,
                CertificationTypeName = dto.CertificationTypeName.Trim(),
                IsActive = dto.IsActive,
                UserId=dto.userId
               // CreatedBy = createdBy
            };

            await _unitOfWork.Repository<CertificationType>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<CertificationTypeDto>(new CertificationTypeDto
            {
                CertificationTypeID = entity.CertificationTypeId,
                CompanyID = entity.CompanyId,
                RegionID = entity.RegionId,
                CertificationTypeName = entity.CertificationTypeName,
                IsActive = entity.IsActive ?? false
            });
        }

        // ================= UPDATE =================
        public async Task<ApiResponse<CertificationTypeDto>> UpdateAsync(
             CreateUpdateCertificationTypeDto dto)
        {
            var entity = await _unitOfWork
                .Repository<CertificationType>()
                .GetByIdAsync(dto.certificationTypeId);

            if (entity == null)
                return new ApiResponse<CertificationTypeDto>(
                    null!, "Not found", false);

            entity.CompanyId = dto.CompanyID;
            entity.RegionId = dto.RegionID;
            entity.CertificationTypeName = dto.CertificationTypeName;
            entity.IsActive = dto.IsActive;
           // entity.ModifiedBy = modifiedBy;

            _unitOfWork.Repository<CertificationType>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<CertificationTypeDto>(
                new CertificationTypeDto
                {
                    CertificationTypeID = entity.CertificationTypeId,
                    CompanyID = entity.CompanyId,
                    RegionID = entity.RegionId,
                    CertificationTypeName = entity.CertificationTypeName,
                    IsActive = entity.IsActive ?? false
                });
        }

        // ================= HARD DELETE =================
        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var entity = await _unitOfWork
                .Repository<CertificationType>()
                .GetByIdAsync(id);

            if (entity == null)
                return new ApiResponse<object>(
                    null!, "Not found", false);

            _unitOfWork.Repository<CertificationType>().Remove(entity);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<object>(
                null!, "Deleted successfully");
        }


        // ================= BULK INSERT =================
        public async Task<ApiResponse<(int inserted, int duplicates, int failed)>> BulkInsertAsync(
            IEnumerable<CreateUpdateCertificationTypeDto> items, int createdBy)
        {
            int inserted = 0, duplicates = 0, failed = 0;

            foreach (var dto in items)
            {
                try
                {
                    var exists = (await _unitOfWork.Repository<CertificationType>()
                        .FindAsync(x =>
                            x.CompanyId == dto.CompanyID &&
                            x.RegionId == dto.RegionID &&
                            x.CertificationTypeName.ToLower() ==
                            dto.CertificationTypeName.ToLower()))
                        .Any();

                    if (exists)
                    {
                        duplicates++;
                        continue;
                    }

                    var entity = new CertificationType
                    {
                        CompanyId = dto.CompanyID,
                        RegionId = dto.RegionID,
                        CertificationTypeName = dto.CertificationTypeName,
                        IsActive = dto.IsActive,
                        CreatedBy = createdBy
                    };

                    await _unitOfWork.Repository<CertificationType>()
                        .AddAsync(entity);

                    inserted++;
                }
                catch
                {
                    failed++;
                }
            }

            await _unitOfWork.CompleteAsync();

            return new ApiResponse<(int, int, int)>(
                (inserted, duplicates, failed),
                $"{inserted} inserted, {duplicates} duplicates, {failed} failed");
        }

    }
}
