using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<IEnumerable<DepartmentDto>>> GetAllAsync(int userId);
        Task<ApiResponse<DepartmentDto?>> GetByIdAsync(int id);
        Task<ApiResponse<DepartmentDto>> CreateAsync(CreateUpdateDepartmentDto dto);
        Task<ApiResponse<DepartmentDto>> UpdateAsync(CreateUpdateDepartmentDto dto);
        Task<ApiResponse<object>> SoftDeleteAsync(int id);
        Task<ApiResponse<(int inserted, int duplicates, int failed)>> BulkInsertAsync(IEnumerable<CreateUpdateDepartmentDto> items, string createdBy);
    }
}
