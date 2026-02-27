using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IShiftAllocationService
    {
        // -------------------------------
        // SHIFT MASTER  (Shift Setup)
        // -------------------------------
        Task<IEnumerable<ShiftMasterDto>> GetAllShiftsAsync(int userId);
        Task<ShiftMasterDto?> GetShiftByIdAsync(int shiftId);
        Task<bool> AddShiftAsync(ShiftMasterDto dto);
        Task<bool> UpdateShiftAsync(ShiftMasterDto dto);
        Task<bool> DeleteShiftAsync(int shiftId);
        Task<bool> ActivateShiftAsync(int shiftId);
        Task<bool> DeactivateShiftAsync(int shiftId);
        Task<IEnumerable<ShiftMasterDto>> GetShiftsForDropdownAsync(int companyId, int regionId);

        // -------------------------------
        // SHIFT ALLOCATION (Main Table)
        // -------------------------------
        Task<IEnumerable<ShiftAllocationDto>> GetAllAllocationsAsync();
        Task<ShiftAllocationDto?> GetAllocationByIdAsync(int id);
        Task<bool> AllocateShiftAsync(ShiftAllocationDto dto);
        Task<bool> UpdateAllocationAsync(ShiftAllocationDto dto);
        Task<EmployeeShiftDto?> GetEmployeeShiftByEmployeeCodeAsync(string employeeCode);
        Task<bool> DeleteAllocationAsync(int id);

        // -------------------------------
        // USER INFO (FullName, EmployeeCode)
        // -------------------------------
        Task<UserReadDto?> GetUserByIdAsync(int userId);

        // -------------------------------
        // AUDIT LOG
        // -------------------------------
        //Task<bool> AddAuditLogAsync(string action, string description, int? createdBy);
    }
}
