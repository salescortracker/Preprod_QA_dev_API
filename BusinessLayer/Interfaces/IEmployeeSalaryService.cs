using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeSalaryService
    {
        Task<List<EmployeeSalaryDto>> GetAllAssignedSalariesAsync(int userId);
        Task<EmployeeSalaryDto> AssignSalaryAsync(EmployeeSalaryDto dto, int userId);

        Task<List<EmployeeSalaryDto>> GetEmployeeSalaryAsync(int employeeId, int userId);
    }
}
