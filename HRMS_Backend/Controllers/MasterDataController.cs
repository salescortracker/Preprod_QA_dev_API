using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IBloodGroupService _bloodGroupService;
        private readonly IDepartmentService _service;
        private readonly IGenderService _genderService;
        private readonly IadminService _adminService;
        private readonly ILogger<MasterDataController> _logger;
        private readonly IDesignationService _designationService;
        private readonly IKpiCategoryService _kpiCategoryService;
        private readonly IEmployeeMasterService _employeeService;
        private readonly ICertificationTypeService _certificationTypeService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IExpenseCategoryService _expensecategoryservice; private readonly IAssetStatusService _assetStatusService;
        private readonly IHelpdeskCategoryAdminService _helpdeskCategoryAdminService;
        private readonly IProjectStatusAdminService _projectStatusAdminService;
        private readonly IPriorityService _priorityService;
        private readonly IAttendanceStatusService _attendanceStatusService;
        private readonly IHolidayListService _holidayListService;
        private readonly IWeekoffService _weekoffService;
        private readonly ILeaveStatusService _leaveStatusService;
        private readonly IPolicyCategoryService _policyCategoryService;
        private readonly IResignationService _resignationService;
        private readonly IEventService _Eventservice;
        public MasterDataController(IEventService Eventservice,IResignationService resignationService,IPolicyCategoryService policyCategoryService,ILeaveStatusService leaveStatusService,IHolidayListService holidayListService, IWeekoffService weekoffService,IAttendanceStatusService attendanceStatusService, IExpenseCategoryService expenseCategoryservice,IDepartmentService service, IDesignationService designationService, IGenderService genderService,IadminService adminService, ILeaveTypeService leaveTypeService,  ILogger<MasterDataController> logger, IKpiCategoryService kpiCategoryService, IEmployeeMasterService employeeService, ICertificationTypeService certificationTypeService, IAssetStatusService assetStatusService, IBloodGroupService bloodGroupService, IHelpdeskCategoryAdminService helpdeskCategoryAdminService, IProjectStatusAdminService projectStatusAdminService, IPriorityService priorityService)
        {
            _service = service;
            _Eventservice = Eventservice;
            _designationService = designationService;
            _genderService = genderService;
            _adminService= adminService;
            _logger = logger;
            _expensecategoryservice = expenseCategoryservice;
            _leaveTypeService = leaveTypeService;
            _kpiCategoryService = kpiCategoryService;
            _employeeService = employeeService;
            _certificationTypeService = certificationTypeService;
            _assetStatusService = assetStatusService;
            _bloodGroupService = bloodGroupService;
            _helpdeskCategoryAdminService = helpdeskCategoryAdminService;
            _projectStatusAdminService = projectStatusAdminService;
            _attendanceStatusService = attendanceStatusService;
            _priorityService = priorityService;
            _holidayListService = holidayListService;
            _weekoffService = weekoffService;
            _leaveStatusService = leaveStatusService;
            _policyCategoryService = policyCategoryService;
            _resignationService = resignationService;
        }
        #region Departments
        // ✅ GET ALL (with optional filters later)
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments(int userId)
        {
            try
            {
                var result = await _service.GetAllAsync(userId);

                if (result == null )
                    return NotFound(new { success = false, message = "No departments found." });

                return Ok(new { success = true, message = "Departments retrieved successfully.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching department list.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while fetching department list." });
            }
        }

        // ✅ GET BY ID
        [HttpGet("GetDepartmentsById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new { success = false, message = $"Department with ID {id} not found." });

                return Ok(new { success = true, message = "Department details retrieved successfully.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving department with ID {id}.");
                return StatusCode(500, new { success = false, message = "An error occurred while fetching department details." });
            }
        }

        // ✅ CREATE
        [HttpPost("createDepartment")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input data. Please check your fields." });

            try
            {
                var createdBy = "system"; // 🔒 TODO: Replace with JWT user later
                var result = await _service.CreateAsync(dto);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new department.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while creating the department." });
            }
        }

        // ✅ UPDATE
        [HttpPost("updateDepartment/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input data." });

            try
            {
                var modifiedBy = "system"; // 🔒 TODO: Replace with JWT user later
                var result = await _service.UpdateAsync(dto);

                if (!result.Success)
                    return NotFound(result);

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating department with ID {id}.");
                return StatusCode(500, new { success = false, message = "An error occurred while updating the department." });
            }
        }

        // ✅ SOFT DELETE
        [HttpDelete("deleteDepartment/{id:int}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var modifiedBy = "system"; // 🔒 TODO: Replace with JWT user later
                var result = await _service.SoftDeleteAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting department with ID {id}.");
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the department." });
            }
        }

        // ✅ BULK INSERT
        [HttpPost("bulk-insert")]
        public async Task<IActionResult> BulkInsert([FromBody] IEnumerable<CreateUpdateDepartmentDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest(new { success = false, message = "No records found to upload." });

            try
            {
                var createdBy = "system"; // 🔒 TODO: Replace with JWT user later
                var result = await _service.BulkInsertAsync(dtos, createdBy);

                return Ok(new { success = true, message = result.Message, insertedCount = result.Data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk department upload.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred during bulk upload." });
            }
        }
        #endregion
        #region Designations
        // ✅ GET ALL
        [HttpGet("GetDesignations")]
        public async Task<IActionResult> GetDesignations(int userId)
        {
            try
            {
                var result = await _designationService.GetAllAsync(userId);

                if (result == null )
                    return NotFound(new { success = false, message = "No designations found." });

                return Ok(new { success = true, message = "Designations retrieved successfully.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching designation list.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while fetching designations." });
            }
        }

        // ✅ GET BY ID
        [HttpGet("GetDesignationById/{id:int}")]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            try
            {
                var result = await _designationService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new { success = false, message = $"Designation with ID {id} not found." });

                return Ok(new { success = true, message = "Designation details retrieved successfully.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving designation with ID {id}.");
                return StatusCode(500, new { success = false, message = "An error occurred while fetching designation details." });
            }
        }

        // ✅ CREATE
        [HttpPost("CreateDesignation")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateDesignationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input data. Please check your fields." });

            try
            {
               // 🔒 TODO: Replace with logged-in user later
                var result = await _designationService.CreateAsync(dto);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new designation.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred while creating the designation." });
            }
        }

        // ✅ UPDATE
        [HttpPost("UpdateDesignation/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateDesignationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input data." });

            try
            {
               // 🔒 TODO: Replace with logged-in user later
                var result = await _designationService.UpdateAsync(id, dto);

                if (!result.Success)
                    return NotFound(result);

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating designation with ID {id}.");
                return StatusCode(500, new { success = false, message = "An error occurred while updating the designation." });
            }
        }

        // ✅ SOFT DELETE
        [HttpPost("DeleteDesignation/{id:int}")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            try
            {
                 // 🔒 TODO: Replace with JWT user later
                var result = await _designationService.SoftDeleteAsync(id);

                if (!result.Success)
                    return NotFound(result);

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting designation with ID {id}.");
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the designation." });
            }
        }
        // ✅ BULK INSERT
        [HttpPost("DesignationBulkInsert")]
        public async Task<IActionResult> BulkInsert([FromBody] IEnumerable<CreateUpdateDesignationDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest(new { success = false, message = "No records found to upload." });

            try
            {
                var createdBy = 1; // TODO: Replace with JWT username later
                var result = await _designationService.BulkInsertAsync(dtos, createdBy);

                return Ok(new
                {
                    success = result.Success,
                    message = result.Message,
                    data = new
                    {
                        inserted = result.Data.inserted,
                        duplicates = result.Data.duplicates,
                        failed = result.Data.failed
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk designation upload.");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred during bulk upload." });
            }
        }

        #endregion
        #region Gender
        /// <summary>
        /// Gender Detail Retrieve
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGenderAll")]
        public async Task<IActionResult> GetGenderAll(int companyId,int regionId,int userId)
        {
            var result = await _genderService.GetAllAsync(companyId, regionId,userId);

            
            if (result==null)
                return NotFound("No gender records found.");

            return Ok(result);
        }
        /// <summary>
        /// Retrieve Gender details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("GetGenderById/{id}")]
        public async Task<IActionResult> GetGenderById(int id)
        {
            var gender = await _genderService.GetGenderByIdAsync(id);
            if (gender == null) return NotFound("Gender not found");
            return Ok(gender);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        [HttpPost("GetGendersearch")]
        public async Task<IActionResult> Search([FromBody] object filter)
        {
            return Ok(await _genderService.SearchGenderAsync(filter));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateGender")]
        public async Task<IActionResult> CreateGender([FromBody] GenderDto dto)
        {
            var result = await _genderService.AddGenderAsync(dto);
            if (result == null)
                return Ok(new { message = "Duplicate Record Found" });
            return Ok(new { message = "Gender created successfully", data = result });
        }

        [HttpPost("UpdateGender")]
        public async Task<IActionResult> UpdateGender([FromBody] GenderDto dto)
        {
            var result = await _genderService.UpdateGenderAsync(dto);
            if (result == null)
                return Ok(new { message = "Duplicate Record Found" });
            return Ok(new { message = "Gender updated successfully", data = result });
        }

        [HttpPost("DeleteGender")]
        public async Task<IActionResult> DeleteGender([FromQuery] int id)
        {
            bool success = await _genderService.DeleteGenderAsync(id);
            if (!success) return NotFound("Gender not found");

            return Ok(new { message = "Gender deleted successfully" });
        }
        #endregion
        #region KPICategory
        // =====================================================
        // KPI CATEGORY
        // =====================================================

        // GET ALL KPI CATEGORIES
        [HttpGet("kpi-categories")]
        public async Task<IActionResult> GetKpiCategories()
        {
            var result = await _kpiCategoryService.GetAll();
            return Ok(result);
        }


        // GET KPI CATEGORY BY ID
        [HttpGet("kpi-categories/{id:int}")]
        public async Task<IActionResult> GetKpiCategoryById(int id)
        {
            var result = await _kpiCategoryService.GetByIdAsync(id);
            return Ok(result);
        }

        // CREATE KPI CATEGORY
        [HttpPost("CreateKpiCategory")]
        public async Task<IActionResult> CreateKpiCategory([FromBody] CreateUpdateKpiCategoryDto dto)
        {
            var result = await _kpiCategoryService.CreateAsync(dto);
            return Ok(result);
        }

        // UPDATE KPI CATEGORY
        [HttpPost("UpdateKpiCategory")]
        public async Task<IActionResult> UpdateKpiCategory([FromBody] CreateUpdateKpiCategoryDto dto)
        {
            var result = await _kpiCategoryService.UpdateAsync(dto);
            return Ok(result);
        }

        // DELETE KPI CATEGORY
        [HttpPost("DeleteKpiCategory")]
        public async Task<IActionResult> DeleteKpiCategory([FromQuery] int id)
        {
            var result = await _kpiCategoryService.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        #endregion

        #region BloodGroup

        #region Get All
        [HttpGet("GetAllBloodGroups")]
        public async Task<IActionResult> GetAllBloodGroups(int companyId)
        {
            var result = await _bloodGroupService.GetAllAsync(companyId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion


        #region Get By Id
        [HttpGet("GetBloodGroupsById/{id}")]
        public async Task<IActionResult> GetBloodGroupsById(int id)
        {
            var result = await _bloodGroupService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        #endregion


        #region Search
        [HttpPost("SearchBloodGroups")]
        //public async Task<IActionResult> SearchBloodGroups([FromBody] BloodGroupDto filter)
        //{
        //    var result = await _bloodGroupService
        //        .SearchbloodgroupAsync(filter);

        //    return Ok(result);
        //}
        #endregion


        #region Add
        [HttpPost("AddBloodGroups")]
        public async Task<IActionResult> AddBloodGroups([FromBody] BloodGroupDto dto)
        {
            var result = await _bloodGroupService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion


        #region Update
        [HttpPut("UpdateBloodGroups")]
        public async Task<IActionResult> UpdateBloodGroups(int id,
            [FromBody] BloodGroupDto dto)
        {
            if (id != dto.BloodGroupID)
                return BadRequest("ID mismatch");

            var result = await _bloodGroupService.UpdateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion


        #region Delete
        [HttpDelete("DeleteBloodGroups/{id}")]
        public async Task<IActionResult> Delete(int id)
        => Ok(await _bloodGroupService.DeleteAsync(id));
        #endregion


        #endregion
        //---------------------------------Employee Master Details---------------------------------//
        #region Employee Master Details


        [HttpGet("GetAllEmployees/{userId}")]
        public async Task<IActionResult> GetAllEmployees(int userId)
        {
            var data = await _employeeService.GetAllEmployees(userId);
            return Ok(data);
        }

        // ================= CREATE =================
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeMasterDto dto)
        {
            if (dto.CreatedBy == null || dto.CreatedBy <= 0)
                return BadRequest("Invalid user.");

            var data = await _employeeService.CreateEmployee(dto);
            return Ok(data);
        }

        // ================= UPDATE =================
        [HttpPost("UpdateEmployee/{id}/{userId}")]
        public async Task<IActionResult> UpdateEmployee(int id, int userId, [FromBody] EmployeeMasterDto dto)
        {
            var data = await _employeeService.UpdateEmployee(id, dto, userId);
            if (data == null)
                return NotFound("Record not found or not authorized.");

            return Ok(data);
        }

        // ================= DELETE =================
        [HttpPost("DeleteEmployee/{id}/{userId}")]
        public async Task<IActionResult> DeleteEmployee(int id, int userId)
        {
            var success = await _employeeService.DeleteEmployee(id, userId);
            if (!success)
                return NotFound("Record not found or not authorized.");

            return Ok(new { message = "Deleted successfully" });
        }

        // ================= MANAGERS =================
        [HttpGet("GetManagers/{userId}")]
        public async Task<IActionResult> GetManagers(int userId)
        {
            var data = await _employeeService.GetManagers(userId);
            return Ok(data);
        }

        #endregion

        //----------------------MY TEAM SECTION----------------------//
        [HttpGet("MyTeam/{managerUserId}")]
        public async Task<IActionResult> GetMyTeam(int managerUserId)
        {
            var tree = await _employeeService.GetMyTeamTreeAsync(managerUserId);
            if (tree == null) return NotFound(new { message = "Manager not found" });
            return Ok(tree);
        }
        // ===================== ASSET STATUS =====================

        /// <summary>
        /// Asset Status CRUD APIs
        /// </summary>
        [HttpGet("asset-status")]
        public async Task<IActionResult> GetAllAssetStatuses(
        [FromQuery] int companyId,
        [FromQuery] int regionId)
        {
            var result = await _assetStatusService.GetAllAsync(companyId, regionId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new asset status
        /// </summary>
        [HttpPost("asset-status")]
        public async Task<IActionResult> CreateAssetStatus([FromBody] AssetStatusDto dto)
        {
            var id = await _assetStatusService.CreateAsync(dto);
            return Ok(id);
        }

        /// <summary>
        /// Updates an existing asset status
        /// </summary>
        [HttpPut("asset-status/{id}")]
        public async Task<IActionResult> UpdateAssetStatus(int id, [FromBody] AssetStatusDto dto)
        {
            dto.AssetStatusId = id;
            var updated = await _assetStatusService.UpdateAsync(dto);
            return updated ? Ok() : NotFound();
        }


        /// <summary>
        /// Deletes (soft delete) an asset status
        /// </summary>
        [HttpDelete("asset-status/{id}")]
        public async Task<IActionResult> DeleteAssetStatus(int id)
        {
            var deleted = await _assetStatusService.DeleteAsync(id);
            return deleted ? Ok() : NotFound();
        }

        #region ===================== CERTIFICATION TYPES =====================

        [HttpGet("certification-types")]
        public async Task<IActionResult> GetCertificationTypes(
            int companyId,
            int regionId)
        {
            var result = await _certificationTypeService
                .GetAllAsync(companyId, regionId);

            return Ok(result!=null?result.Data:result);
        }

        [HttpGet("GetCmpregionAllAsync")]
        public async Task<IActionResult> GetCmpregionAllAsync(
           int companyId,
           int regionId)
        {
            var result = await _certificationTypeService
                .GetCmpregionAllAsync(companyId, regionId);

            return Ok(result != null ? result.Data : result);
        }

        [HttpGet("certification-types/{id:int}")]
        public async Task<IActionResult> GetCertificationTypeById(int id)
        {
            var result = await _certificationTypeService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("CreateCertificationType")]
        public async Task<IActionResult> CreateCertificationType(
            [FromBody] CreateUpdateCertificationTypeDto dto
            )
        {
            var result = await _certificationTypeService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("UpdateCertificationType")]
        public async Task<IActionResult> UpdateCertificationType(
           
            [FromBody] CreateUpdateCertificationTypeDto dto
            )
        {
            var result = await _certificationTypeService
                .UpdateAsync( dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("DeleteCertificationType")]
        public async Task<IActionResult> DeleteCertificationType([FromQuery] int id)
        {
            var result = await _certificationTypeService.DeleteAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("certification-types/bulk")]
        public async Task<IActionResult> BulkInsertCertificationTypes(
            [FromBody] IEnumerable<CreateUpdateCertificationTypeDto> dtos,
            [FromQuery] int createdBy)
        {
            var result = await _certificationTypeService
                .BulkInsertAsync(dtos, createdBy);

            return Ok(result);
        }

        #endregion
        #region LeaveType
        [HttpGet("GetLeaveType")]
        public async Task<IActionResult> GetLeaveType()
        {
            // call service without parameters
            var data = await _leaveTypeService.GetLeaveTypesAsync();
            return Ok(data);
        }
        [HttpGet("GetCRLeaveTypesAsync")]
        public async Task<IActionResult> GetCRLeaveTypesAsync(
    int companyId,
    int regionId)
        {
            var result = await _leaveTypeService.GetCRLeaveTypesAsync(companyId, regionId);
            return Ok(result);
        }

        [HttpPost("CreateLeaveType")]
        public async Task<IActionResult> CreateLeaveType([FromBody] LeaveTypeDto dto)
        {
            var result = await _leaveTypeService.CreateLeaveTypeAsync(dto);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("UpdateLeaveType")]
        public async Task<IActionResult> UpdateLeaveType([FromBody] LeaveTypeDto dto)
        {
            var result = await _leaveTypeService.UpdateLeaveTypeAsync(dto);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("DeleteLeaveType")]
        public async Task<IActionResult> DeleteLeaveType([FromQuery] int id)
        {
            var result = await _leaveTypeService.DeleteLeaveTypeAsync(id);

            if (!result)
                return NotFound("Leave Type not found or already deleted");

            return Ok(new { message = "Leave Type deleted successfully" });
        }



        #endregion
        #region expenseCategory
        [HttpGet("GetexpenseCategoryAll")]
        public async Task<IActionResult> GetexpenseCategoryAll(int companyId, int regionId)
        {
            var result = await _expensecategoryservice.GetAllAsync(companyId, regionId);
            return Ok(result);
        }

        [HttpPost("AddexpenseCategory")]
        public async Task<IActionResult> AddexpenseCategory([FromBody] ExpenseCategoryDto dto)
        {
            var result = await _expensecategoryservice.AddAsync(dto);
            return Ok(result);
        }

        [HttpPost("UpdateexpenseCategory")]
        public async Task<IActionResult> UpdateexpenseCategory([FromBody] ExpenseCategoryDto dto)
        {
            var result = await _expensecategoryservice.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpPost("DeleteexpenseCategory")]
        public async Task<IActionResult> DeleteexpenseCategory([FromQuery] int id)
        {
            var result = await _expensecategoryservice.DeleteAsync(id);
            return Ok(result);
        }
        #endregion

        // ===============================
        // GET ALL
        // ===============================
        [HttpGet("project-status")]
        public async Task<IActionResult> GetAllProjects([FromQuery] int userId)
        {
            var result = await _projectStatusAdminService.GetAllProject(userId);
            return Ok(result);
        }

        // ===============================
        // GET BY ID
        // ===============================
        [HttpGet("project-status/{id}")]
        public async Task<IActionResult> GetByIdProject(int id)
        {
            var result = await _projectStatusAdminService.GetByIdProjectAsync(id);
            return Ok(result);
        }

        // ===============================
        // CREATE
        // ===============================
        [HttpPost("project-status")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectStatusDto dto)
        {
            var result = await _projectStatusAdminService.CreateProjectAsync(dto);
            return Ok(result);
        }

        // ===============================
        // UPDATE
        // ===============================
        [HttpPut("project-status/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectStatusDto dto)
        {
            dto.ProjectStatusId = id;
            var result = await _projectStatusAdminService.UpdateProjectAsync(dto);
            return Ok(result);
        }

        // ===============================
        // DELETE
        // ===============================
        [HttpDelete("project-status/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectStatusAdminService.DeleteProjectAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        #region Priority
        // =====================================================
        // PRIORITY
        // =====================================================

        [HttpGet("priorities")]
        public async Task<IActionResult> GetPriorities([FromQuery] int userId)
        {
            var result = await _priorityService.GetAll(userId);
            return Ok(result);
        }

        [HttpGet("priorities/{id:int}")]
        public async Task<IActionResult> GetPriorityById(int id)
        {
            var result = await _priorityService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreatePriority")]
        public async Task<IActionResult> CreatePriority([FromBody] PriorityDto dto)
        {
            var result = await _priorityService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPost("UpdatePriority")]
        public async Task<IActionResult> UpdatePriority([FromBody] PriorityDto dto)
        {
            var result = await _priorityService.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpPost("DeletePriority")]
        public async Task<IActionResult> DeletePriority([FromQuery] int id)
        {
            var result = await _priorityService.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        #endregion

        #region Helpdesk
        // ===============================
        // GET ALL
        // ===============================
        [HttpGet("helpdesk-category")]
        public async Task<IActionResult> GetAll([FromQuery] int userId)
        {
            var result = await _helpdeskCategoryAdminService.GetAll(userId);
            return Ok(result);
        }

        // ===============================
        // GET BY ID
        // ===============================
        [HttpGet("helpdesk-category/{id}")]
        public async Task<IActionResult> GetByIds(int id)
        {
            var result = await _helpdeskCategoryAdminService.GetByIdAsync(id);
            return Ok(result);
        }

        // ===============================
        // CREATE
        // ===============================
        [HttpPost("helpdesk-category")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateHelpdeskCategoryDto dto)
        {
            var result = await _helpdeskCategoryAdminService.CreateAsync(dto);
            return Ok(result);
        }

        // ===============================
        // UPDATE
        // ===============================
        [HttpPut("helpdesk-category/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateHelpdeskCategoryDto dto)
        {
            dto.HelpdeskCategoryID = id;
            var result = await _helpdeskCategoryAdminService.UpdateAsync(dto);
            return Ok(result);
        }

        // ===============================
        // DELETE
        // ===============================
        [HttpDelete("helpdesk-category/{id}")]
        public async Task<IActionResult> helpdeskcategory(int id)
        {
            var result = await _helpdeskCategoryAdminService.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        #endregion

        #region AttendanceStatus

        [HttpGet("GetAllAttendanceStatus")]
        public async Task<IActionResult> GetAll(int companyId, int regionId)
        {
            var result = await _attendanceStatusService.GetAllAsync(companyId, regionId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetAttendanceStatusById/{id}")]
        public async Task<IActionResult> GetAttendanceStatusById(int id)
        {
            var result = await _attendanceStatusService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("AddAttendanceStatus")]
        public async Task<IActionResult> AddAttendanceStatus(
      [FromBody] AttendanceStatusDto dto)
        {
            var result = await _attendanceStatusService.CreateAsync(dto);

            if (!result.Success)
                return Conflict(result);   // 409 for duplicate

            return CreatedAtAction(
                nameof(GetAttendanceStatusById),
                new { id = result.Data.AttendanceStatusId },
                result);
        }

        [HttpPut("UpdateAttendanceStatus")]
        public async Task<IActionResult> UpdateAttendanceStatus(int id, [FromBody] AttendanceStatusDto dto)
        {

            var result = await _attendanceStatusService.UpdateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("DeleteAttendanceStatus/{id}")]
        public async Task<IActionResult> DeleteAttendanceStatus(int id)
        {
            return Ok(await _attendanceStatusService.DeleteAsync(id));
        }

        #endregion

        #region Weekoff

        [HttpGet("weekoff-list")]
        public async Task<IActionResult> GetWeekoffList([FromQuery] int userId)
        {
            var result = await _weekoffService.GetAll(userId);
            return Ok(result);
        }

        [HttpGet("weekoff-list/{id:int}")]
        public async Task<IActionResult> GetWeekoffById(int id)
        {
            var result = await _weekoffService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateWeekoff")]
        public async Task<IActionResult> CreateWeekoff([FromBody] WeekoffDto dto)
        {
            var result = await _weekoffService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPost("UpdateWeekoff")]
        public async Task<IActionResult> UpdateWeekoff([FromBody] WeekoffDto dto)
        {
            var result = await _weekoffService.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpPost("DeleteWeekoff")]
        public async Task<IActionResult> DeleteWeekoff([FromQuery] int id)
        {
            var result = await _weekoffService.DeleteAsync(id);
            return Ok(result);
        }

        #endregion
        #region HolidayList

        [HttpGet("holiday-list")]
        public async Task<IActionResult> GetHolidayList([FromQuery] int userId)
        {
            var result = await _holidayListService.GetAll(userId);
            return Ok(result);
        }

        [HttpGet("holiday-list/{id:int}")]
        public async Task<IActionResult> GetHolidayById(int id)
        {
            var result = await _holidayListService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateHoliday")]
        public async Task<IActionResult> CreateHoliday([FromBody] CreateUpdateHolidayListDto dto)
        {
            var result = await _holidayListService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPost("UpdateHoliday")]
        public async Task<IActionResult> UpdateHoliday([FromBody] CreateUpdateHolidayListDto dto)
        {
            var result = await _holidayListService.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpPost("DeleteHoliday")]
        public async Task<IActionResult> DeleteHoliday([FromQuery] int id)
        {
            var result = await _holidayListService.DeleteAsync(id);
            return Ok(result);
        }

        #endregion
        #region LeaveStatus

        #region Get All
        [HttpGet("GetAllLeaveStatus")]
        public async Task<IActionResult> GetAllLeaveStatus(int companyId, int regionId)
        {

            var result = await _leaveStatusService
                .GetAllLeaveStatusAsync(companyId, regionId);

            if (result == null)
                return StatusCode(500, "Service returned null response");

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion


        #region Get By Id
        [HttpGet("GetLeaveStatusById/{id}")]
        public async Task<IActionResult> GetLeaveStatusById(int id)
        {
            var result = await _leaveStatusService
                .GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        #endregion


        #region Add
        [HttpPost("AddLeaveStatus")]
        public async Task<IActionResult> AddLeaveStatus(
            [FromBody] LeaveStatusDto dto)
        {
            var result = await _leaveStatusService
                .CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion


        #region Update
        [HttpPut("UpdateLeaveStatus")]
        public async Task<IActionResult> UpdateLeaveStatus(
            int id,
            [FromBody] LeaveStatusDto dto)
        {
            if (id != dto.LeaveStatusID)
                return BadRequest("ID mismatch");

            var result = await _leaveStatusService
                .UpdateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion


        #region Delete
        [HttpDelete("DeleteLeaveStatus/{id}")]
        public async Task<IActionResult> DeleteLeaveStatus(int id)
        {
            //return Ok(await _leaveStatusService.DeleteAsync(id));
            var result = await _leaveStatusService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);

        }
        #endregion

        #endregion
        #region PolicyCategory

        [HttpGet("policy-category")]
        public async Task<IActionResult> GetPolicyCategories([FromQuery] int userId)
        {
            var result = await _policyCategoryService.GetAll(userId);
            return Ok(result);
        }

        [HttpGet("policy-category/{id:int}")]
        public async Task<IActionResult> GetPolicyCategoryById(int id)
        {
            var result = await _policyCategoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CreatePolicyCategory")]
        public async Task<IActionResult> CreatePolicyCategory([FromBody] CreateUpdatePolicyCategoryDto dto)
        {
            var result = await _policyCategoryService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPost("UpdatePolicyCategory")]
        public async Task<IActionResult> UpdatePolicyCategory([FromBody] CreateUpdatePolicyCategoryDto dto)
        {
            var result = await _policyCategoryService.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpPost("DeletePolicyCategory")]
        public async Task<IActionResult> DeletePolicyCategory([FromQuery] int id)
        {
            var result = await _policyCategoryService.DeleteAsync(id);
            return Ok(result);
        }

        #endregion
        //-------------------------------RESIGNATIONMASTER-------------------------------//

        #region Resignations

        [HttpGet("GetResignations")]
        public IActionResult GetResignations(int companyId, int regionId)
        {
            var data = _resignationService.GetAll(companyId, regionId);
            return Ok(data);
        }

        [HttpGet("GetResignationById/{id:int}")]
        public IActionResult GetResignationById(int id)
        {
            var data = _resignationService.GetById(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost("CreateResignation")]
        public IActionResult CreateResignation([FromForm] ResignationDto dto, [FromQuery] int userId)
        {
            var success = _resignationService.Create(dto, userId);
            return success ? Ok(new { message = "Created successfully" }) : BadRequest();
        }

        [HttpPost("UpdateResignation/{id:int}")]
        public IActionResult UpdateResignation(int id, [FromForm] ResignationDto dto, [FromQuery] int userId)
        {
            var success = _resignationService.Update(id, dto, userId);
            return success ? Ok(new { message = "Updated successfully" }) : NotFound();
        }

        [HttpPost("DeleteResignation/{id:int}")]
        public IActionResult DeleteResignation(int id, [FromQuery] int userId)
        {
            var success = _resignationService.Delete(id, userId);
            return success ? Ok(new { message = "Deleted successfully" }) : NotFound();
        }

        #endregion

        // ✅ 1️⃣ Get All Events
        [HttpGet("GetEventsAll")]
        public async Task<IActionResult> GetEventsAll()
        {
            var events = await _Eventservice.GetAllAsync();
            return Ok(events);
        }

        // ✅ 2️⃣ Get Event By Id
        [HttpGet("GetEventById/{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventData = await _Eventservice.GetByIdAsync(id);

            if (eventData == null)
                return NotFound(new { message = "Event not found" });

            return Ok(eventData);
        }

        // ✅ 3️⃣ Create Event
        [HttpPost("CreateEvents")]
        public async Task<IActionResult> CreateEvents([FromBody] EventDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEvent = await _Eventservice.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdEvent.EventId },
                createdEvent);
        }

        // ✅ 4️⃣ Update Event
        [HttpPost("UpdateEvents")]
        public async Task<IActionResult> UpdateEvents([FromBody] EventDTO dto)
        {
           
            var updatedEvent = await _Eventservice.UpdateAsync(dto);

            if (updatedEvent == null)
                return NotFound(new { message = "Event not found" });

            return Ok(updatedEvent);
        }

        // ✅ 5️⃣ Delete Event
        [HttpPost("DeleteEvents")]
        public async Task<IActionResult> DeleteEvents([FromQuery] int id)
        {
            var deleted = await _Eventservice.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Event not found" });

            return Ok(new { message = "Event deleted successfully" });
        }
    }
}
