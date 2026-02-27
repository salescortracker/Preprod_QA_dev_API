using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserManagementController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IRegionService _regionService;
        private readonly IUserService _userService;
        private readonly IMenuMasterService _menuService;
        private readonly IRoleMasterService _roleService;
        private readonly IMenuRoleService _menuRoleService;
        private readonly IadminService _adminService;
        private readonly IMaritalStatusService _maritalStatusService;
        private readonly IEmployeeMasterService _employeeService;
        private readonly HRMSContext _hRMSContext;
        public UserManagementController(HRMSContext hrmscontext,ICompanyService companyService, IRegionService regionService, IUserService userService
            , IMenuMasterService menuService, IMaritalStatusService maritalStatusService, IRoleMasterService roleService, IMenuRoleService menuRoleService, IadminService adminService, IEmployeeMasterService employeeService)
        {
            _companyService = companyService;
            _regionService = regionService;
            _userService = userService;
            _menuService = menuService;
            _roleService = roleService;
            _menuRoleService = menuRoleService;
            _adminService = adminService;
            _hRMSContext = hrmscontext;
            _maritalStatusService = maritalStatusService;
            _employeeService = employeeService;
        }   
        public class BulkInsertRequest
        {
            public string EntityName { get; set; }
            public List<object> Data { get; set; }
        }
        public class BulkInsertResult<T>
        {
            public int InsertedCount { get; set; }
            public int DuplicateCount { get; set; }
            public List<T> InsertedRecords { get; set; }
            public List<T> DuplicateRecords { get; set; }
        }

        #region Company Details
        /// <summary>
        /// Retrieves a list of all companies.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all companies from the data
        /// source.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of companies.  Returns an HTTP 200 status code with
        /// the list of companies if successful.</returns>
        [HttpGet]
        [Route("GetCompany")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var companies = await _companyService.GetAllCompaniesAsync(userId);
            return Ok(companies);
        }
        /// <summary>
        /// Retrieves a company by its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the company details. Ensure
        /// the <paramref name="id"/> corresponds to a valid company record.</remarks>
        /// <param name="id">The unique identifier of the company to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the company data if found; otherwise, a <see
        /// cref="NotFoundResult"/> if the company does not exist.</returns>
        [HttpGet]
        [Route("GetCompanyById")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }
        /// <summary>
        /// Searches for companies based on the specified filter criteria.
        /// </summary>
        /// <remarks>The filter object must be structured according to the requirements of the underlying
        /// search service. Ensure that the filter contains valid criteria to avoid unexpected results.</remarks>
        /// <param name="filter">An object containing the filter criteria for the search. The structure and fields of the filter object
        /// depend on the implementation of the search service.</param>
        /// <returns>An <see cref="IActionResult"/> containing the search results. The result is a collection of companies that
        /// match the specified filter criteria.</returns>
        [HttpPost]
        [Route("GetCompanySearch")]
        public async Task<IActionResult> Search([FromBody] object filter)
        {
            var companies = await _companyService.SearchCompaniesAsync(filter);
            return Ok(companies);
        }
        /// <summary>
        /// Creates a new company and returns the created resource with its location.
        /// </summary>
        /// <remarks>This method uses the HTTP POST verb to create a new company. The created resource's
        /// URI is included in the response.</remarks>
        /// <param name="dto">The data transfer object containing the details of the company to create.</param>
        /// <returns>A <see cref="CreatedAtActionResult"/> containing the details of the created company and the URI of the
        /// resource.</returns>
        [HttpPost]
        [Route("SaveCompany")]
        public async Task<IActionResult> Create([FromBody] CompanyDto dto)
        {
            var company = await _companyService.AddCompanyAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = company.CompanyId }, company);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateCompany/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyDto dto)
        {
            var updated = await _companyService.UpdateCompanyAsync(id, dto);
            return Ok(updated);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("DeleteCompany/{id}")]
       
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpPost]
        [Route("BulkInsert")]
        public async Task<IActionResult> BulkInsert([FromBody] BulkInsertRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.EntityName) || request.Data == null || !request.Data.Any())
                    return BadRequest(new { Success = false, Message = "Invalid request. No data provided." });

                switch (request.EntityName.ToLower())
                {
                    case "company":

                        var companies = new List<CompanyDto>();

                        foreach (var item in request.Data)
                        {
                            // Handle both stringified and object JSON
                            CompanyDto? company = null;

                            switch (item)
                            {
                                case string jsonString:
                                    company = JsonConvert.DeserializeObject<CompanyDto>(jsonString);
                                    break;

                                case JObject jobj:
                                    company = jobj.ToObject<CompanyDto>();
                                    break;

                                case JsonElement jsonElement:
                                    if (jsonElement.ValueKind == JsonValueKind.String)
                                    {
                                        var innerJson = jsonElement.GetString();
                                        if (!string.IsNullOrWhiteSpace(innerJson))
                                            company = JsonConvert.DeserializeObject<CompanyDto>(innerJson);
                                    }
                                    else
                                    {
                                        var json = jsonElement.GetRawText();
                                        company = JsonConvert.DeserializeObject<CompanyDto>(json);
                                    }
                                    break;
                            }

                            if (company != null)
                                companies.Add(company);
                        }

                        if (!companies.Any())
                            return BadRequest(new { Success = false, Message = "Failed to parse company data. Invalid JSON format." });

                        // ✅ Call service layer
                        var result = await _companyService.AddCompaniesAsync(companies);

                        return Ok(new
                        {
                            Success = true,
                            Message = $"{result.Count()} companies inserted successfully. {result.Count()} duplicate(s) skipped.",
                            Summary = result
                        });

                    // Add more entity cases as needed
                    

                    default:
                        return BadRequest(new { Success = false, Message = "Unsupported entity type." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message ?? "An unexpected error occurred. Please contact IT Administrator."
                });
            }
        }




        #endregion
        #region Region Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRegion")]
        public async Task<IActionResult> GetRegion(int userId)
        {
            var regions = await _regionService.GetAllRegionsAsync(userId);
            return Ok(regions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRegionById")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            var region = await _regionService.GetRegionByIdAsync(id);
            if (region == null) return NotFound();
            return Ok(region);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveRegion")]
        public async Task<IActionResult> SaveRegion([FromBody] object model)
        {
            var region = await _regionService.AddRegionAsync(model);
            return CreatedAtAction(nameof(GetRegionById), new { id = region.RegionID }, region);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateRegion/{id}")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] object model)
        {
            var region = await _regionService.UpdateRegionAsync(id, model);
            return Ok(region);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteRegion/{id}")]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var result = await _regionService.DeleteRegionAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRegionSearch")]
        public async Task<IActionResult> GetRegionSearch([FromBody] object filter)
        {
            var regions = await _regionService.SearchRegionsAsync(filter);
            return Ok(regions);
        }
        #endregion
        #region User Details
        [HttpGet("GetAllUsers/{userId}")]
        public async Task<IActionResult> GetAllUsers(int userId)
        {
            var users = await _userService.GetAllUsersAsync(userId);
            return Ok(users);
        }
        [HttpGet("GetcmpregAllUsers")]
        public async Task<IActionResult> GetcmpregAllUsers([FromQuery] int companyId,int regionId)
        {
            var users = await _userService.GetcmpregAllUsersAsync(companyId,regionId);
            return Ok(users);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
        {
            if (userDto == null)
                return BadRequest("Invalid user data");

            var createdUser = await _userService.CreateUserAsync(userDto);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
        }

      

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserCreateDto user)
        {
            var updatedUser = await _userService.UpdateUserAsync(user);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }



        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                var user = await _userService.VerifyLoginAsync(model.Email, model.Password);

                if (user == null)
                    return Unauthorized(new { message = "Invalid username or password" });

                return Ok(new { message = "Login successful", user });
            }
            catch (Exception ex)
            {
                // Handle unexpected API-level issues
                Console.WriteLine($"Login failed: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your login." });
            }
        }

        #endregion
        #region Menu Master Details
        /// <summary>
        /// Get all menus
        /// </summary>
        [HttpGet("GetAllMenus")]
        public async Task<IActionResult> GetAllMenus()
       {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(menus);
        }

        /// <summary>
        /// Get menu by ID
        /// </summary>
        [HttpGet("GetMenuById/{id:int}")]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
                return NotFound(new { message = "Menu not found" });

            return Ok(menu);
        }

        /// <summary>
        /// Search menus dynamically by MenuName, ParentMenuID, IsActive, etc.
        /// </summary>
        [HttpPost("SearchMenus")]
        public async Task<IActionResult> SearchMenus([FromBody] object filter)
        {
            var results = await _menuService.SearchMenusAsync(filter);
            return Ok(results);
        }

        /// <summary>
        /// Create a new menu
        /// </summary>
        [HttpPost("CreateMenu")]
        public async Task<IActionResult> CreateMenu([FromBody] MenuMasterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Example: retrieve CreatedBy from token later if you have authentication
            var createdBy = 1; // placeholder for now
            var menu = await _menuService.AddMenuAsync(dto, createdBy);
            return CreatedAtAction(nameof(GetMenuById), new { id = menu.MenuID }, menu);
        }

        /// <summary>
        /// Update an existing menu
        /// </summary>
        [HttpPost("UpdateMenu/{id:int}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuMasterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modifiedBy = 1; // placeholder for now
            try
            {
                var updatedMenu = await _menuService.UpdateMenuAsync(id, dto, modifiedBy);
                return Ok(updatedMenu);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete menu by ID
        /// </summary>
        [HttpPost("DeleteMenu/{id:int}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var deleted = await _menuService.DeleteMenuAsync(id);
            if (!deleted)
                return NotFound(new { message = "Menu not found" });

            return NoContent();
        }

        /// <summary>
        /// Get all active menus
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveMenus()
        {
            var activeMenus = await _menuService.GetActiveMenusAsync();
            return Ok(activeMenus);
        }
        #endregion
        #region Role Details
        // ✅ GET: api/RoleMaster
          // ✅ GET: api/RoleMaster
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles([FromQuery] int userId)
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync(userId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ✅ GET: api/RoleMaster/{id}
        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }

        // ✅ POST: api/RoleMaster
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleMasterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRole = await _roleService.AddRoleAsync(dto);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.RoleId }, createdRole);
        }

        // ✅ PUT: api/RoleMaster/{id}
        [HttpPost("UpdateRole/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleMasterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(id, dto);
                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ✅ DELETE: api/RoleMaster/{id}
        [HttpPost("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var deleted = await _roleService.DeleteRoleAsync(id);
            if (!deleted)
                return NotFound(new { message = "Role not found or already deleted" });

            return Ok(new { message = "Role deleted successfully" });
        }

        // ✅ POST: api/RoleMaster/search
        [HttpPost("search")]
        public async Task<IActionResult> SearchRoles(
            [FromBody] object filter,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            var roles = await _roleService.SearchRolesAsync(filter, pageNumber, pageSize, sortBy, isDescending);
            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = roles.Count(),
                Data = roles
            });
        }
        #endregion
        #region menurolemaster
        /// <summary>
        /// Assign permissions for multiple roles.
        /// </summary>
        [HttpPost("AssignMultipleRoles")]
        public async Task<IActionResult> AssignPermissionsToMultipleRoles([FromBody] List<RolePermissionRequestDto> rolePermissions)
        {
            if (rolePermissions == null || !rolePermissions.Any())
                return BadRequest("Role permissions data cannot be empty.");

            var success = await _menuRoleService.AssignPermissionsToMultipleRolesAsync(rolePermissions);
            return success ? Ok(new { Message = "Permissions assigned successfully." }) : StatusCode(500, "Failed to assign permissions.");
        }

        /// <summary>
        /// Get permissions for multiple roles.
        /// </summary>
        [HttpPost("GetPermissionsForMultipleRoles")]
        public async Task<IActionResult> GetPermissionsForMultipleRoles([FromBody] List<int> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
                return BadRequest("Role IDs list cannot be empty.");

            var result = await _menuRoleService.GetPermissionsForMultipleRolesAsync(roleIds);
            return Ok(result);
        }
        /// <summary>
        /// Assign permissions for a single role.
        /// </summary>
        [HttpPost("assign-permissions/{roleId}")]
        public async Task<IActionResult> AssignPermissionsToRole(int roleId, [FromBody] List<MenuRoleDto> permissions)
        {
            if (permissions == null || !permissions.Any())
                return BadRequest("Permissions list cannot be empty.");

            var success = await _menuRoleService.AssignPermissionsToRoleAsync(roleId, permissions);
            if (success)
                return Ok(new { message = "Permissions assigned successfully." });

            return StatusCode(500, "Failed to assign permissions.");
        }

        /// <summary>
        /// Get all assigned permissions for a role.
        /// </summary>
        [HttpGet("get-permissions/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRole(int roleId)
        {
            var result = await _menuRoleService.GetPermissionsByRoleAsync(roleId);
            return Ok(result);
        }
        [HttpGet("GetAllMenusByRoleId/{roleId}")]
        public async Task<IActionResult> GetAllMenusByRoleId(int roleId)
        {
            try
            {
                var permissions = await _menuRoleService.GetAllMenusByRoleId(roleId);

                if (permissions == null || !permissions.Any())
                    return NotFound(new { message = "No permissions found for this role." });

                return Ok(permissions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching permissions: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving permissions." });
            }
        }
        #endregion
        #region Relationship Details
        /// <summary>
        /// Get all relationships by user, company & region id
        /// </summary>
        [HttpGet("GetAllRelationShip")]
        public async Task<IActionResult> GetAllRelationShip(
            int userId,
            int companyId,
            int regionId)
        {
            var result = await _adminService.GetAllrelatiopnshipByUserAsync(companyId, regionId);

            //if (!result.Any())
            //    return Ok("No relationships found.");

            return Ok(result);
        }

        [HttpGet("GetAllUserIdrelatiopnshipByUserAsync")]
        public async Task<IActionResult> GetAllUserIdrelatiopnshipByUserAsync(
           int userId
          )
        {
            var result = await _adminService.GetAllUserIdrelatiopnshipByUserAsync(userId);

            //if (!result.Any())
            //    return Ok("No relationships found.");

            return Ok(result);
        }



        /// <summary>
        /// Add new Relationship
        /// </summary>
        [HttpPost("AddRelationship")]
        public async Task<IActionResult> AddRelationship([FromBody] RelationshipDto relationship)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.AddrelatiopnshipAsync(relationship);
            return Ok(result);
        }

        /// <summary>
        /// Update Relationship by Id
        /// </summary>
        [HttpPost("UpdateRelationship")]
        public async Task<IActionResult> UpdateRelationship([FromBody] RelationshipDto relationship)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.UpdaterelatiopnshipAsync(relationship);

            if (result == null)
                return NotFound("Relationship record not found to update.");

            return Ok(result);
        }

        /// <summary>
        /// Soft delete relationship by Id
        /// </summary>
        [HttpPost("DeleteRelationship")]
        public async Task<IActionResult> DeleteRelationship(
         [FromQuery]   int relationshipId
            )
        {
            var result = await _adminService.Deleterelatiopnship(relationshipId);

            if (!result)
                return NotFound("Relationship record not found to delete.");

            return Ok("Deleted Successfully");
        }
        #endregion
        #region gender Details
        [HttpGet("GetAllgenderByUserAsync")]
        public async Task<IActionResult> GetAllgenderByUserAsync(
       int userId,
       int companyId,
       int regionId)
        {
            var result = await _adminService.GetAllgenderByUserAsync(companyId, regionId);

            if (!result.Any())
                return NotFound("No gender records found.");

            return Ok(result);
        }

        [HttpPost("Addgender")]
        public async Task<IActionResult> Addgender([FromBody] Gender gender)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.AddgenderAsync(gender);
            return Ok(result);
        }

        [HttpPost("Updategender")]
        public async Task<IActionResult> Updategender([FromBody] Gender gender)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _adminService.UpdategenderAsync(gender);

            if (result == null)
                return NotFound("Gender record not found to update.");

            return Ok(result);
        }

        [HttpPost("Deletegender")]
        public async Task<IActionResult> Deletegender([FromQuery]
            int genderId
            )
        {
            var result = await _adminService.DeletegenderAsync(genderId);

            if (!result)
                return NotFound("Gender record not found to delete.");

            return Ok("Deleted Successfully");
        }
        #endregion
        [HttpPost("DemoRequest")]
        public async Task<IActionResult> DemoRequest([FromBody] DemoRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = new DataAccessLayer.DBContext.User
                {
                    FullName = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.Phone,
                    CompanyName = dto.Company,
                    Module = dto.Module,
                    Type = "Demo",
                    CompanyId = 1,
                    RegionId = 2,
                    RoleId = 1,
                    PasswordHash = "Demo@123", // In real scenarios, hash the password properly

                    CreatedDate = DateTime.Now
                };

                _hRMSContext.Users.Add(entity);
               await _hRMSContext.SaveChangesAsync();
                // ✅ Send Welcome Email
                await _userService.SendWelcomeEmailAsync(
                   entity, entity.PasswordHash
                );
                return Ok(new { message = "Demo Request submitted successfully. Please check your email with login credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _userService.ChangePasswordAsync(dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        #region Marital Status

        // POST: api/MaritalStatus/get-all
        [HttpPost("getall")]
        public async Task<IActionResult> GetAllMaritalStatus(int UserId)
        {
            var data = await _maritalStatusService.GetAllAsync(UserId);
            return Ok(data);
        }

        // POST: api/MaritalStatus/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateMaritalStatus(
            [FromForm] int companyId,
            [FromForm] int regionId,
            [FromForm] string maritalStatusName,
            [FromForm] string? description,
            [FromForm] bool isActive,
            [FromForm] int userId)
        {
            var dto = new MaritalStatusDto
            {
                CompanyId = companyId,
                RegionId = regionId,
                MaritalStatusName = maritalStatusName,
                Description = description,
                IsActive = isActive
                ,UserId=userId
            };

            await _maritalStatusService.CreateAsync(dto);
            return Ok(new { message = "Marital Status created successfully" });
        }

        // POST: api/MaritalStatus/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateMaritalStatus(
            [FromForm] int id,
            [FromForm] int companyId,
            [FromForm] int regionId,
            [FromForm] string maritalStatusName,
            [FromForm] string? description,
            [FromForm] bool isActive)
        {
            var dto = new MaritalStatusDto
            {
                CompanyId = companyId,
                RegionId = regionId,
                MaritalStatusName = maritalStatusName,
                Description = description,
                IsActive = isActive
            };

            var result = await _maritalStatusService.UpdateAsync(dto);
            return result
                ? Ok(new { message = "Marital Status updated successfully" })
                : NotFound();
        }

        // POST: api/MaritalStatus/delete
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMaritalStatus([FromForm] int id)
        {
            var result = await _maritalStatusService.DeleteAsync(id);
            return result
                ? Ok(new { message = "Marital Status deleted successfully" })
                : NotFound();
        }

        #endregion

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = await _userService.SendOtpAsync(dto.Email);
            return Ok(result);
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var result = await _userService.VerifyOtpAsync(dto.Email, dto.Otp);
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto.Email, dto.NewPassword);
            return Ok(result);
        }

        //---------------------------------Employee Master Details---------------------------------//
        #region Employee Master Details


        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var data = await _employeeService.GetAllEmployees();
            return Ok(data);
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeMasterDto dto)
        {
            var data = await _employeeService.CreateEmployee(dto);
            return Ok(data);
        }

        [HttpPost("UpdateEmployee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeMasterDto dto)
        {
            var data = await _employeeService.UpdateEmployee(id, dto);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _employeeService.DeleteEmployee(id);
            if (!success) return NotFound();
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet("GetManagers")]
        public async Task<IActionResult> GetManagers()
        {
            var data = await _employeeService.GetManagers();
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





    
}
}
