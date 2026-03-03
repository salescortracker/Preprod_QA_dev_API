using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class EmployeeMasterService: IEmployeeMasterService
    {
        private readonly HRMSContext _context;

        public EmployeeMasterService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeMasterDto>> GetAllEmployees(int userId)
        {
            return await _context.EmployeeMasters
                .Where(e => e.CreatedBy == userId)
                .Select(e => new EmployeeMasterDto
                {
                    EmployeeMasterId = e.EmployeeMasterId,
                    FullName = e.FullName,
                    Role = e.Role,
                    RoleId = e.RoleId,
                    Department = e.Department,
                    ManagerId = e.ManagerId
                })
                .ToListAsync();
        }

        public async Task<EmployeeMasterDto> CreateEmployee(EmployeeMasterDto dto)
        {
            var entity = new EmployeeMaster
            {
                FullName = dto.FullName,
                Role = dto.Role,
                RoleId = dto.RoleId,
                Department = dto.Department,
                ManagerId = dto.ManagerId,
                CreatedBy = dto.CreatedBy
            };

            _context.EmployeeMasters.Add(entity);
            await _context.SaveChangesAsync();

            dto.EmployeeMasterId = entity.EmployeeMasterId;
            return dto;
        }

        public async Task<EmployeeMasterDto> UpdateEmployee(int id, EmployeeMasterDto dto, int userId)
        {
            var entity = await _context.EmployeeMasters
                .FirstOrDefaultAsync(e => e.EmployeeMasterId == id && e.CreatedBy == userId);

            if (entity == null)
                return null;

            entity.FullName = dto.FullName;
            entity.Role = dto.Role;
            entity.RoleId = dto.RoleId;
            entity.Department = dto.Department;
            entity.ManagerId = dto.ManagerId;
            entity.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteEmployee(int id, int userId)
        {
            var entity = await _context.EmployeeMasters
                .FirstOrDefaultAsync(e => e.EmployeeMasterId == id && e.CreatedBy == userId);

            if (entity == null)
                return false;

            _context.EmployeeMasters.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ManagerDropdownDto>> GetManagers(int userId)
        {
            return await _context.Users
                .Where(u => u.CreatedBy == userId)
                .Select(u => new ManagerDropdownDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName
                })
                .ToListAsync();
        }
        // ================== MY TEAM TREE ==================
        public async Task<MyTeamDto> GetMyTeamTreeAsync(int managerUserId)
        {
            // 1️⃣ Try to find EmployeeMaster for this manager
            var managerEmployee = await _context.EmployeeMasters
                .FirstOrDefaultAsync(e => e.CreatedBy == managerUserId);

            // 2️⃣ Load all employees
            var allEmployees = await _context.EmployeeMasters
                .Select(e => new MyTeamDto
                {
                    EmployeeMasterId = e.EmployeeMasterId,
                    FullName = e.FullName,
                    Role = e.Role,
                    ManagerId = e.ManagerId,
                    Subordinates = new List<MyTeamDto>()
                })
                .ToListAsync();

            MyTeamDto managerNode;

            if (managerEmployee != null)
            {
                // Manager exists in EmployeeMaster
                managerNode = allEmployees.First(e => e.EmployeeMasterId == managerEmployee.EmployeeMasterId);
            }
            else
            {
                // Manager not in EmployeeMaster → create top node from Users table
                var managerUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == managerUserId);

                if (managerUser == null)
                    return null; // Will return 404 in controller

                managerNode = new MyTeamDto
                {
                    EmployeeMasterId = managerUser.UserId,
                    FullName = managerUser.FullName,
                    Role = null,
                    ManagerId = null,
                    Subordinates = new List<MyTeamDto>()
                };
            }

            // 3️⃣ Build tree recursively
            BuildSubordinates(managerNode, allEmployees);

            return managerNode;
        }

        // Recursion function (unchanged)
        private void BuildSubordinates(MyTeamDto manager, List<MyTeamDto> allEmployees)
        {
            var subs = allEmployees.Where(e => e.ManagerId == manager.EmployeeMasterId).ToList();
            foreach (var sub in subs)
            {
                manager.Subordinates.Add(sub);
                BuildSubordinates(sub, allEmployees); // recursion
            }
        }

    }
}
