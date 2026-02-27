using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class ExpenseCategoryService: IExpenseCategoryService
    {
        private readonly HRMSContext _context;

        public ExpenseCategoryService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<ExpenseCategoryDto>>> GetAllAsync(
            int companyId, int regionId)
        {
            var list = await _context.ExpenseCategories
              //  .Where(x => x.CompanyId == companyId && x.RegionId == regionId)
                .OrderBy(x => x.SortOrder)
                .Select(x => new ExpenseCategoryDto
                {
                    ExpenseCategoryID = x.ExpenseCategoryId,
                    ExpenseCategoryName = x.ExpenseCategoryName,
                    IsActive = x.IsActive,
                    SortOrder = x.SortOrder,
                    Description = x.Description,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId
                })
                .ToListAsync();

            return new ApiResponse<IEnumerable<ExpenseCategoryDto>>(list);
        }

        public async Task<ApiResponse<bool>> AddAsync(ExpenseCategoryDto dto)
        {
            var entity = new ExpenseCategory
            {
                ExpenseCategoryName = dto.ExpenseCategoryName,
                Description = dto.Description,
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedDate = DateTime.UtcNow
            };

            _context.ExpenseCategories.Add(entity);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(ExpenseCategoryDto dto)
        {
            var entity = await _context.ExpenseCategories
                .FirstOrDefaultAsync(x => x.ExpenseCategoryId == dto.ExpenseCategoryID);

            if (entity == null)
                return new ApiResponse<bool>(false, "Record not found");

            entity.ExpenseCategoryName = dto.ExpenseCategoryName;
            entity.Description = dto.Description;
            entity.SortOrder = dto.SortOrder;
            entity.IsActive = dto.IsActive;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new ApiResponse<bool>(true);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int expenseCategoryId)
        {
            var entity = await _context.ExpenseCategories
                .FirstOrDefaultAsync(x => x.ExpenseCategoryId == expenseCategoryId);

            if (entity == null)
                return new ApiResponse<bool>(false, "Record not found");

            _context.ExpenseCategories.Remove(entity);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true);
        }
    }
}
