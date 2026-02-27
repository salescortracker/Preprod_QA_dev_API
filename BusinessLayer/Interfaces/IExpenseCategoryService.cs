using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<ApiResponse<IEnumerable<ExpenseCategoryDto>>> GetAllAsync(int companyId, int regionId);
        Task<ApiResponse<bool>> AddAsync(ExpenseCategoryDto dto);
        Task<ApiResponse<bool>> UpdateAsync(ExpenseCategoryDto dto);
        Task<ApiResponse<bool>> DeleteAsync(int expenseCategoryId);
    }
}
