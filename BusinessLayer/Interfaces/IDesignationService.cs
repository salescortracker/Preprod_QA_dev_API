using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IDesignationService
    {
        Task<ApiResponse<IEnumerable<DesignationDTO>>> GetAllAsync(int userId);
        Task<ApiResponse<DesignationDTO?>> GetByIdAsync(int id);
        Task<ApiResponse<DesignationDTO>> CreateAsync(CreateUpdateDesignationDto dto);
        Task<ApiResponse<DesignationDTO>> UpdateAsync(int id, CreateUpdateDesignationDto dto);
        Task<ApiResponse<object>> SoftDeleteAsync(int id);
        Task<ApiResponse<(int inserted, int duplicates, int failed)>> BulkInsertAsync(IEnumerable<CreateUpdateDesignationDto> items, int createdBy);

    }
}
