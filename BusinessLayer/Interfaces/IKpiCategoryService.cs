using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IKpiCategoryService
    {
        Task<ApiResponse<IEnumerable<KpiCategoryDto>>> GetAll();

        Task<ApiResponse<KpiCategoryDto?>> GetByIdAsync(int id);

        Task<ApiResponse<string>> CreateAsync(CreateUpdateKpiCategoryDto dto);

        Task<ApiResponse<string>> UpdateAsync(CreateUpdateKpiCategoryDto dto);

        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}
