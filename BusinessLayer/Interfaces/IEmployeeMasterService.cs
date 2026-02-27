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
        Task<List<EmployeeMasterDto>> GetAllEmployees();
        Task<EmployeeMasterDto> CreateEmployee(EmployeeMasterDto dto);
        Task<EmployeeMasterDto> UpdateEmployee(int id, EmployeeMasterDto dto);
        Task<bool> DeleteEmployee(int id);
        Task<List<ManagerDropdownDto>> GetManagers();

        //----------------------MY TEAM SECTION----------------------//
        Task<MyTeamDto> GetMyTeamTreeAsync(int managerUserId); // NEW TREE METHOD
    }
}
