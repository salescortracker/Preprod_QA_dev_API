using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPolicyCategoryService
    {
        Task<ApiResponse<IEnumerable<CreateUpdatePolicyCategoryDto>>> GetAll(int userId);
        Task<ApiResponse<CreateUpdatePolicyCategoryDto?>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(CreateUpdatePolicyCategoryDto dto);
        Task<ApiResponse<string>> UpdateAsync(CreateUpdatePolicyCategoryDto dto);
        Task<ApiResponse<string>> DeleteAsync(int id);

    }
}
