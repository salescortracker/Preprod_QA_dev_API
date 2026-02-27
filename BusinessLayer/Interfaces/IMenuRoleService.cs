using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IMenuRoleService
    {
        Task<IEnumerable<MenuRoleDto>> GetAllPermissionsAsync();
        Task<IEnumerable<MenuRoleDto>> GetPermissionsByRoleAsync(int roleId);
        Task<MenuRoleDto?> GetPermissionByIdAsync(int id);
        Task<MenuRoleDto> AddPermissionAsync(MenuRoleDto dto);
        Task<MenuRoleDto> UpdatePermissionAsync(int id, MenuRoleDto dto);
        Task<bool> DeletePermissionAsync(int id);
        Task<bool> AssignPermissionsToRoleAsync(int roleId, List<MenuRoleDto> permissions);
        Task<bool> AssignPermissionsToMultipleRolesAsync(List<RolePermissionRequestDto> rolePermissions);
        Task<IEnumerable<MenuRoleDto>> GetPermissionsForMultipleRolesAsync(List<int> roleIds);
        Task<IEnumerable<MenuRoleMasterDto>> GetAllMenusByRoleId(int roleId);

    }
}
