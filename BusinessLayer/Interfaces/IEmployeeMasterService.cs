using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEmployeeMasterService
    {
        Task<List<EmployeeMasterDto>> GetAllEmployees(int userId);
        Task<EmployeeMasterDto> CreateEmployee(EmployeeMasterDto dto);
        Task<EmployeeMasterDto> UpdateEmployee(int id, EmployeeMasterDto dto, int userId);
        Task<bool> DeleteEmployee(int id, int userId);
        Task<List<ManagerDropdownDto>> GetManagers(int userId);

        Task<MyTeamDto> GetMyTeamTreeAsync(int managerUserId);
    }
}
