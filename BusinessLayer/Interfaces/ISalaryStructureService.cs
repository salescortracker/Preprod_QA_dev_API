using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ISalaryStructureService
    {
        Task<List<SalaryStructureDto>> GetAllSalaryStructuresAsync(int userId);
        Task<SalaryStructureDto?> GetSalaryStructureByIdAsync(int id, int userId);
        Task<SalaryStructureDto> CreateSalaryStructureAsync(SalaryStructureDto dto, int userId);
        Task<SalaryStructureDto?> UpdateSalaryStructureAsync(int id, SalaryStructureDto dto, int userId);
        Task<bool> DeleteSalaryStructureAsync(int id, int userId);
    }
}
