using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ISalaryComponentService
    {
        Task<List<SalaryComponentDto>> GetAllAsync(int userId);
        Task<SalaryComponentDto?> GetByIdAsync(int id, int userId);
        Task<SalaryComponentDto> CreateAsync(SalaryComponentDto dto, int userId);
        Task<SalaryComponentDto?> UpdateAsync(int id, SalaryComponentDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
