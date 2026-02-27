using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class MenuRoleService : IMenuRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuRoleDto>> GetAllPermissionsAsync()
        {
            var entities = await _unitOfWork.Repository<MenuRoleMaster>().GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<IEnumerable<MenuRoleDto>> GetPermissionsByRoleAsync(int roleId)
        {
            var entities = await _unitOfWork.Repository<MenuRoleMaster>().FindAsync(x => x.RoleId == roleId);
            return entities.Select(MapToDto);
        }
        public async Task<IEnumerable<MenuRoleMasterDto>> GetAllMenusByRoleId(int roleId)
        {
            try
            {
                var menuRoleRepo = _unitOfWork.Repository<MenuRoleMaster>();
                var menuRepo = _unitOfWork.Repository<MenuMaster>();
                var roleRepo = _unitOfWork.Repository<RoleMaster>();

                // Get IQueryable for each table
                var menuRoles = await menuRoleRepo.GetAllAsync();
                var menus = await menuRepo.GetAllAsync();
                var roles = await roleRepo.GetAllAsync();

                // Perform join in-memory using LINQ-to-Objects
                var result = (from mr in menuRoles
                              join m in menus on mr.MenuId equals m.MenuId
                              join r in roles on mr.RoleId equals r.RoleId
                              where mr.RoleId == roleId && mr.IsActive==true && m.IsActive==true
                              orderby m.OrderNo
                              select new MenuRoleMasterDto
                              {
                                  MenuRoleId = mr.MenuRoleId,
                                  RoleId = mr.RoleId,
                                  RoleName = r.RoleName,
                                  MenuId = m.MenuId,
                                  MenuName = m.MenuName,
                                  MenuUrl = m.Url,
                                  OrderNo = m.OrderNo,
                                  icon=m.Icon,
                                  ParentId = m.ParentMenuId,
                                  CanView = mr.CanView,
                                  CanAdd = mr.CanAdd,
                                  CanEdit = mr.CanEdit,
                                  CanDelete = mr.CanDelete,
                                  IsActive = mr.IsActive
                              }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching menus for RoleId {roleId}: {ex.Message}");
                return Enumerable.Empty<MenuRoleMasterDto>();
            }
        }



        public async Task<MenuRoleDto?> GetPermissionByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<MenuRoleMaster>().GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<MenuRoleDto> AddPermissionAsync(MenuRoleDto dto)
        {
            var entity = new MenuRoleMaster
            {
                RoleId = dto.RoleId,
                MenuId = dto.MenuId,
                CanView = dto.CanView,
                CanAdd = dto.CanAdd,
                CanEdit = dto.CanEdit,
                CanDelete = dto.CanDelete,
                CanApprove = dto.CanApprove,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.Now
            };

            await _unitOfWork.Repository<MenuRoleMaster>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        public async Task<MenuRoleDto> UpdatePermissionAsync(int id, MenuRoleDto dto)
        {
            var entity = await _unitOfWork.Repository<MenuRoleMaster>().GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Permission not found.");

            entity.CanView = dto.CanView;
            entity.CanAdd = dto.CanAdd;
            entity.CanEdit = dto.CanEdit;
            entity.CanDelete = dto.CanDelete;
            entity.CanApprove = dto.CanApprove;
            entity.IsActive = dto.IsActive;
            entity.ModifiedDate = DateTime.Now;

            _unitOfWork.Repository<MenuRoleMaster>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var entity = await _unitOfWork.Repository<MenuRoleMaster>().GetByIdAsync(id);
            if (entity == null)
                return false;

            _unitOfWork.Repository<MenuRoleMaster>().Remove(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        /// <summary>
        /// Assign all permissions for a single role at once.
        /// </summary>
        /// <summary>
        /// Assign permissions for multiple roles at once.
        /// </summary>
        public async Task<bool> AssignPermissionsToMultipleRolesAsync(List<RolePermissionRequestDto> rolePermissions)
        {
            if (rolePermissions == null || !rolePermissions.Any())
                throw new ArgumentException("Role permissions list cannot be empty.");

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var roleData in rolePermissions)
                {
                    int roleId = roleData.RoleId;
                    var permissions = roleData.Permissions;

                    // Remove existing permissions for this role
                    var existing = await _unitOfWork.Repository<MenuRoleMaster>().FindAsync(x => x.RoleId == roleId);
                    foreach (var old in existing)
                        _unitOfWork.Repository<MenuRoleMaster>().Remove(old);

                    // Add new permissions
                    foreach (var dto in permissions)
                    {
                        var entity = new MenuRoleMaster
                        {
                            RoleId = roleId,
                            MenuId = dto.MenuId,
                            CanView = dto.CanView,
                            CanAdd = dto.CanAdd,
                            CanEdit = dto.CanEdit,
                            CanDelete = dto.CanDelete,
                            CanApprove = dto.CanApprove,
                            IsActive = dto.IsActive,
                            CreatedDate = DateTime.Now
                        };

                        await _unitOfWork.Repository<MenuRoleMaster>().AddAsync(entity);
                    }
                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Get permissions for multiple roles.
        /// </summary>
        public async Task<IEnumerable<MenuRoleDto>> GetPermissionsForMultipleRolesAsync(List<int> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
                throw new ArgumentException("Role IDs list cannot be empty.");

            var permissions = await _unitOfWork.Repository<MenuRoleMaster>()
                .FindAsync(x => roleIds.Contains(x.RoleId));

            return permissions.Select(x => new MenuRoleDto
            {
                MenuId = x.MenuId,
                CanView = x.CanView,
                CanAdd = x.CanAdd,
                CanEdit = x.CanEdit,
                CanDelete = x.CanDelete,
                CanApprove = x.CanApprove,
                IsActive = x.IsActive
            }).ToList();
        }

        public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<MenuRoleDto> permissions)
        {
            if (permissions == null || !permissions.Any())
                throw new ArgumentException("Permission list cannot be empty.");

            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Remove old permissions for this role
                var existingPermissions = await _unitOfWork.Repository<MenuRoleMaster>().FindAsync(x => x.RoleId == roleId);
                foreach (var old in existingPermissions)
                    _unitOfWork.Repository<MenuRoleMaster>().Remove(old);

                // Add new permissions
                foreach (var dto in permissions)
                {
                    var entity = new MenuRoleMaster
                    {
                        RoleId = roleId,
                        MenuId = dto.MenuId,
                        CanView = dto.CanView,
                        CanAdd = dto.CanAdd,
                        CanEdit = dto.CanEdit,
                        CanDelete = dto.CanDelete,
                        CanApprove = dto.CanApprove,
                        IsActive = dto.IsActive,
                        CreatedDate = DateTime.Now
                    };

                    await _unitOfWork.Repository<MenuRoleMaster>().AddAsync(entity);
                }

                await _unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error while assigning permissions", ex);
            }
        }
        // ✅ Mapper
        private static MenuRoleDto MapToDto(MenuRoleMaster e)
        {
            return new MenuRoleDto
            {
                MenuRoleId = e.MenuRoleId,
                RoleId = e.RoleId,
                MenuId = e.MenuId,
                CanView = e.CanView,
                CanAdd = e.CanAdd,
                CanEdit = e.CanEdit,
                CanDelete = e.CanDelete,
                CanApprove = e.CanApprove,
                IsActive = e.IsActive
            };
        }
    }
}
