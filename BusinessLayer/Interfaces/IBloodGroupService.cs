using BusinessLayer.Common;
using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;

namespace BusinessLayer.Interfaces
{
    public interface IBloodGroupService
    {
        Task<ApiResponse<IEnumerable<BloodGroupDto>>>
     GetAllAsync(int companyId);

        Task<ApiResponse<IEnumerable<BloodGroupDto>>>
            GetByIdAsync(int id);

        Task<ApiResponse<BloodGroupDto>>
            CreateAsync(BloodGroupDto dto);

        Task<ApiResponse<BloodGroupDto>>
            UpdateAsync(BloodGroupDto dto);

        Task<ApiResponse<bool>>
            DeleteAsync(int id);
    }
}
