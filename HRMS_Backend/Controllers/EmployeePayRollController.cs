using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePayRollController : ControllerBase
    {
        private readonly ISalaryComponentService _salaryComponentService;
        private readonly ISalaryStructureService _structureService;
        private readonly IEmployeeSalaryService _employeeSalaryService;
        private readonly IPayrollService _payrollService;
        public EmployeePayRollController(ISalaryComponentService salaryComponentService,
           ISalaryStructureService structureService, IEmployeeSalaryService employeeSalaryService,
           IPayrollService payrollService)
        {
            _salaryComponentService = salaryComponentService;
            _structureService = structureService;
            _employeeSalaryService = employeeSalaryService;
            _payrollService = payrollService;
        }

        #region Salary Component Code 

        // GET: api/EmployeePayRoll/components/5
        [HttpGet("components/{userId}")]
        public async Task<IActionResult> GetAllComponents(int userId)
        {
            var data = await _salaryComponentService.GetAllAsync(userId);
            return Ok(data);
        }

        // GET: api/EmployeePayRoll/component/1/5
        [HttpGet("component/{id}/{userId}")]
        public async Task<IActionResult> GetComponent(int id, int userId)
        {
            var data = await _salaryComponentService.GetByIdAsync(id, userId);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // POST: api/EmployeePayRoll/components/5
        [HttpPost("components/{userId}")]
        public async Task<IActionResult> CreateComponent(int userId, [FromBody] SalaryComponentDto dto)
        {
            var result = await _salaryComponentService.CreateAsync(dto, userId);
            return Ok(result);
        }

        // PUT: api/EmployeePayRoll/components/1/5
        [HttpPut("components/{id}/{userId}")]
        public async Task<IActionResult> UpdateComponent(int id, int userId, [FromBody] SalaryComponentDto dto)
        {
            var result = await _salaryComponentService.UpdateAsync(id, dto, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/EmployeePayRoll/components/1/5
        [HttpDelete("components/{id}/{userId}")]
        public async Task<IActionResult> DeleteComponent(int id, int userId)
        {
            var result = await _salaryComponentService.DeleteAsync(id, userId);
            if (!result) return NotFound();
            return Ok(new { message = "Deleted Successfully" });
        }

        #endregion

        #region Salary Sstructure Component code 

        [HttpGet("GetAllSalaryStructures/{userId}")]
        public async Task<IActionResult> GetAllSalaryStructures(int userId)
        {
            var result = await _structureService.GetAllSalaryStructuresAsync(userId);
            return Ok(result);
        }

        [HttpGet("GetSalaryStructureById/{id}/{userId}")]
        public async Task<IActionResult> GetSalaryStructureById(int id, int userId)
        {
            var result = await _structureService.GetSalaryStructureByIdAsync(id, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("CreateSalaryStructure/{userId}")]
        public async Task<IActionResult> CreateSalaryStructure(int userId, [FromBody] SalaryStructureDto dto)
        {
            var result = await _structureService.CreateSalaryStructureAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut("UpdateSalaryStructure/{id}/{userId}")]
        public async Task<IActionResult> UpdateSalaryStructure(int id, int userId, [FromBody] SalaryStructureDto dto)
        {
            var result = await _structureService.UpdateSalaryStructureAsync(id, dto, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("DeleteSalaryStructure/{id}/{userId}")]
        public async Task<IActionResult> DeleteSalaryStructure(int id, int userId)
        {
            var result = await _structureService.DeleteSalaryStructureAsync(id, userId);
            if (!result) return NotFound();

            return Ok(new { message = "Deleted Successfully" });
        }

        #endregion

        #region Assign Employee Salary Code

        [HttpGet("employee-salary/{userId}")]
        public async Task<IActionResult> GetAllAssignedSalaries(int userId)
        {
            var result = await _employeeSalaryService.GetAllAssignedSalariesAsync(userId);
            return Ok(result);
        }

        [HttpPost("employee-salary/{userId}")]
        public async Task<IActionResult> AssignSalary(int userId, [FromBody] EmployeeSalaryDto dto)
        {
            var result = await _employeeSalaryService.AssignSalaryAsync(dto, userId);
            return Ok(result);
        }


        [HttpGet("employee-salary/{employeeId}/{userId}")]
        public async Task<IActionResult> GetEmployeeSalary(int employeeId, int userId)
        {
            var result = await _employeeSalaryService.GetEmployeeSalaryAsync(employeeId, userId);
            return Ok(result);
        }

        #endregion

        #region Employee Salary Calculations

        [HttpPost("preview/{userId}")]
        public async Task<IActionResult> PreviewPayroll(int userId, [FromBody] ProcessPayrollRequestDto dto)
        {
            var result = await _payrollService.PreviewPayrollAsync(dto, userId);
            return Ok(result);
        }

        [HttpPost("process/{userId}")]
        public async Task<IActionResult> ProcessPayroll(int userId, [FromBody] ProcessPayrollRequestDto dto)
        {
            var result = await _payrollService.ProcessPayrollAsync(dto, userId);
            return Ok(new
            {
                success = true,
                message = result
            });
        }

        [HttpGet("{month}/{year}/{userId}")]
        public async Task<IActionResult> GetPayroll(int month, int year, int userId)
        {
            var result = await _payrollService.GetPayrollByMonthAsync(month, year, userId);
            return Ok(result);
        }

        #endregion
    }

}

