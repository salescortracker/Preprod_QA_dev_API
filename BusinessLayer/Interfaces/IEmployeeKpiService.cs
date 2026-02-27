using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeKpiService
    {
        Task<int> CreateKpiAsync(EmployeeKpiDto kpiDto);
        Task<List<EmployeeKpiDto>> GetAllKpisAsync();

        Task<List<EmployeeKpiDto>> GetKpisByUserAsync(int userId);

        Task<bool> UpdateKpiAsync(EmployeeKpiDto kpiDto);
        Task<bool> DeleteKpiAsync(int kpiId);
    }
}
