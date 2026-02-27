using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class RoleMasterService : IRoleMasterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HRMSContext context;

        public RoleMasterService(IUnitOfWork unitOfWork, HRMSContext _context)
        {
            _unitOfWork = unitOfWork;
            context = _context;
        }

        // Get all roles
        public async Task<IEnumerable<RoleMasterDto>> GetAllRolesAsync(int userId)
        {
            try
            {

                var roles = await (
                    from r in context.RoleMasters
                    where r.UserId == userId
                    join c in context.Companies on r.CompanyId equals c.CompanyId into rc
                    from company in rc.DefaultIfEmpty()

                    join reg in context.Regions on r.RegionId equals reg.RegionId into rr
                    from region in rr.DefaultIfEmpty()

                    select new RoleMasterDto
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName ?? "",
                        RoleDescription = r.RoleDescription ?? "",
                        IsActive = r.IsActive,
                        CompanyId = r.CompanyId,
                        RegionId = r.RegionId,
                        CompanyName = company != null ? company.CompanyName : "",
                        RegionName = region != null ? region.RegionName : ""
                    }
                ).ToListAsync();

                return roles;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // Get role by ID
        public async Task<RoleMasterDto?> GetRoleByIdAsync(int id)
        {
            var role = await _unitOfWork.Repository<RoleMaster>().GetByIdAsync(id);
            return role == null ? null : MapToDto(role);
        }

        // Add new role
        public async Task<RoleMasterDto> AddRoleAsync(RoleMasterDto dto)
        {
            try
            {
                var entity = new RoleMaster
                {
                    RoleName = dto.RoleName,
                    RoleDescription = dto.RoleDescription,
                    IsActive = dto.IsActive,
                    CreatedBy = dto.CreatedBy,
                    CompanyId = dto.CompanyId,
                    RegionId = dto.RegionId,
                    UserId = dto.UserId,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.Repository<RoleMaster>().AddAsync(entity);
                await _unitOfWork.CompleteAsync();

                return MapToDto(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Update existing role
        public async Task<RoleMasterDto> UpdateRoleAsync(int id, RoleMasterDto dto)
        {
            var entity = await _unitOfWork.Repository<RoleMaster>().GetByIdAsync(id);
            if (entity == null) throw new Exception("Role not found");

            entity.RoleName = dto.RoleName;
            entity.RoleDescription = dto.RoleDescription;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;

            _unitOfWork.Repository<RoleMaster>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        // Delete role
        public async Task<bool> DeleteRoleAsync(int id)
        {
            var entity = await _unitOfWork.Repository<RoleMaster>().GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Repository<RoleMaster>().Remove(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        // Search + pagination + sorting
        public async Task<IEnumerable<RoleMasterDto>> SearchRolesAsync(object filter, int pageNumber = 1, int pageSize = 10, string? sortBy = null, bool isDescending = false)
        {
            var props = filter.GetType().GetProperties();
            var allRoles = await _unitOfWork.Repository<RoleMaster>().GetAllAsync();
            var query = allRoles.AsQueryable();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(filter);

                if (value != null)
                {
                    switch (name)
                    {
                        case nameof(RoleMaster.RoleName):
                            query = query.Where(r => r.RoleName != null && r.RoleName.Contains(value.ToString()!));
                            break;
                        case nameof(RoleMaster.RoleDescription):
                            query = query.Where(r => r.RoleDescription != null && r.RoleDescription.Contains(value.ToString()!));
                            break;
                        case nameof(RoleMaster.IsActive):
                            bool isActive = Convert.ToBoolean(value);
                            query = query.Where(r => r.IsActive == isActive);
                            break;
                    }
                }
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var results = query.ToList();
            return results.Select(r => MapToDto(r));
        }

        // Mapper
        private RoleMasterDto MapToDto(RoleMaster r)
        {
            return new RoleMasterDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                RoleDescription = r.RoleDescription,
                IsActive = r.IsActive,
                CreatedBy = r.CreatedBy,
                CreatedAt = r.CreatedAt,
                ModifiedBy = r.ModifiedBy,
                ModifiedAt = r.ModifiedAt
            };
        
    }
    }
}
