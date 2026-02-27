using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRoleMasterService
    {
      
        Task<IEnumerable<RoleMasterDto>> GetAllRolesAsync(int userId);
        Task<RoleMasterDto?> GetRoleByIdAsync(int id);
        Task<RoleMasterDto> AddRoleAsync(RoleMasterDto dto);
        Task<RoleMasterDto> UpdateRoleAsync(int id, RoleMasterDto dto);

        Task<bool> DeleteRoleAsync(int id);
        Task<IEnumerable<RoleMasterDto>> SearchRolesAsync(object filter, int pageNumber = 1, int pageSize = 10, string? sortBy = null, bool isDescending = false);
    }
}
