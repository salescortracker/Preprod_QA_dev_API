using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IShiftAllocationService _shiftAllocationService;
        private readonly IClockInOutService _clockInOutService;
        private readonly ITimesheetService _timesheetService;
        private readonly IMissedPunchService _service;
        private readonly IWorkFromHomeRequestService _workfromhomeservice;
        private readonly ICompanyNewsService _companyNewsService;
        private readonly ICompanyPolicyService _companyPolicyService;

        public AttendanceController(ICompanyPolicyService companyPolicyService,IShiftAllocationService shiftAllocationService, IClockInOutService clockInOutService, ITimesheetService timesheetService, IMissedPunchService service,IWorkFromHomeRequestService workfromhomeservice, ICompanyNewsService companyNewsService)
        {
            
            _shiftAllocationService = shiftAllocationService;
            _clockInOutService = clockInOutService;
            _timesheetService = timesheetService;
            _service = service;
            _workfromhomeservice = workfromhomeservice;
            _companyNewsService = companyNewsService;
            _companyPolicyService = companyPolicyService;
        }
        #region ShiftAllocation

        [HttpGet("GetAllShifts")]
        public async Task<IActionResult> GetAllShifts(int userId)
        {
            var data = await _shiftAllocationService.GetAllShiftsAsync(userId);
            return Ok(data);
        }

        [HttpGet("GetShiftById/{shiftId}")]
        public async Task<IActionResult> GetShiftById(int shiftId)
        {
            var result = await _shiftAllocationService.GetShiftByIdAsync(shiftId);
            if (result == null) return NotFound("Shift not found");
            return Ok(result);
        }

        [HttpPost("AddShift")]
        public async Task<IActionResult> AddShift([FromBody] ShiftMasterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var status = await _shiftAllocationService.AddShiftAsync(dto);

            if (status)
                return Ok(new { success = true });

            return BadRequest("Failed");
        }



        [HttpPut("UpdateShift")]
        public async Task<IActionResult> UpdateShift([FromBody] ShiftMasterDto dto)
        {
            var status = await _shiftAllocationService.UpdateShiftAsync(dto);
            return status ? Ok(status) : NotFound("Shift not found");
        }

        [HttpDelete("DeleteShift/{shiftId}")]
        public async Task<IActionResult> DeleteShift(int shiftId)
        {
            var status = await _shiftAllocationService.DeleteShiftAsync(shiftId);
            if (status)
                return Ok(new { success = true, message = "Shift deleted successfully" });
            else
                return NotFound(new { success = false, message = "Shift not found" });
        }

        [HttpPut("ActivateShift/{shiftId}")]
        public async Task<IActionResult> ActivateShift(int shiftId)
        {
            var status = await _shiftAllocationService.ActivateShiftAsync(shiftId);
            return status ? Ok("Shift activated") : NotFound("Shift not found");
        }

        [HttpPut("DeactivateShift/{shiftId}")]
        public async Task<IActionResult> DeactivateShift(int shiftId)
        {
            var status = await _shiftAllocationService.DeactivateShiftAsync(shiftId);
            return status ? Ok("Shift deactivated") : NotFound("Shift not found");
        }

        [HttpGet("GetShiftsForDropdown")]
        public async Task<IActionResult> GetShiftsForDropdown([FromQuery] int companyId, [FromQuery] int regionId)
        {
            if (companyId == 0 || regionId == 0)
                return BadRequest("CompanyId and RegionId are required.");

            var shifts = await _shiftAllocationService.GetShiftsForDropdownAsync(companyId, regionId);
            return Ok(shifts);
        }


        // ===========================================================
        //                  SHIFT ALLOCATION API
        // ===========================================================

        [HttpGet("GetAllAllocations/{userId}")]
        public async Task<IActionResult> GetAllAllocations(int userId)
        {
            var list = await _shiftAllocationService.GetAllAllocationsAsync(userId);
            return Ok(list);
        }

        [HttpGet("GetAllocationById/{id}")]
        public async Task<IActionResult> GetAllocationById(int id)
        {
            var result = await _shiftAllocationService.GetAllocationByIdAsync(id);
            if (result == null) return NotFound("Allocation not found");
            return Ok(result);
        }

        [HttpPost("AllocateShift")]
        public async Task<IActionResult> AllocateShift([FromBody] ShiftAllocationDto dto)
        {
            var status = await _shiftAllocationService.AllocateShiftAsync(dto);
            return   Ok(status) ;
        }

        [HttpPost("UpdateAllocation")]
        public async Task<IActionResult> UpdateAllocation([FromBody] ShiftAllocationDto dto)
        {
            var status = await _shiftAllocationService.UpdateAllocationAsync(dto);
            return status ? Ok("Allocation updated successfully") : NotFound("Allocation not found");
        }

        [HttpDelete("DeleteAllocation/{id}")]
        public async Task<IActionResult> DeleteAllocation(int id)
        {
            var status = await _shiftAllocationService.DeleteAllocationAsync(id);
            return status ? Ok("Allocation deleted") : NotFound("Allocation not found");
        }





        #endregion
        // 🔹 GET: api/ClockInOut
        [HttpGet("GetclockinoutAll")]
        public async Task<IActionResult> GetclockinoutAll()
        {
            var result = await _clockInOutService.GetAllAsync();
            return Ok(result);
        }

        // 🔹 GET: api/ClockInOut/5
        [HttpGet("GetclockinoutById{id:int}")]
        public async Task<IActionResult> GetclockinoutById(int id)
        {
            var result = await _clockInOutService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Record not found" });

            return Ok(result);
        }

        // 🔹 GET: api/ClockInOut/today
        [HttpGet("GetTodayByEmployee")]
        public async Task<IActionResult> GetTodayByEmployee(
            [FromQuery] string employeeCode,
            [FromQuery] int companyId,
            [FromQuery] int regionId)
        {
            var result = await _clockInOutService
                .GetTodayByEmployeeAsync(employeeCode, companyId, regionId);

            return Ok(result);
        }

        // 🔹 POST: api/ClockInOut
        [HttpPost("AddclockinOut")]
        public async Task<IActionResult> AddclockinOut(
            [FromBody] ClockInOutCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔐 Example: get logged-in userId
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var result = await _clockInOutService.AddAsync(dto, userId);

            return Ok(result);
        }

        // 🔹 DELETE: api/ClockInOut/5
        [HttpPost("DeleteClockinOut")]
        public async Task<IActionResult> DeleteClockinOut([FromQuery]int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var success = await _clockInOutService.DeleteAsync(id, userId);

            if (!success)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }
        [HttpGet("ShiftallocationName/{employeeCode}")]
        public async Task<IActionResult> GetEmployeeShift(string employeeCode)
        {
            var result = await _shiftAllocationService.GetEmployeeShiftByEmployeeCodeAsync(employeeCode);

            if (result == null)
                return NotFound(new { message = "Shift not allocated for this employee" });

            return Ok(result);
        }

        [HttpGet("GetLoggedInUser/{userId}")]
        public async Task<IActionResult> GetLoggedInUser(int userId)
        {
            var data = await _timesheetService.GetLoggedInUserAsync(userId);
            return Ok(data);
        }
        [HttpPost("SaveTimesheet")]
        public async Task<IActionResult> SaveTimesheet([FromForm] TimesheetRequestDto dto)
        {
            string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "Timesheets");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // 📎 File upload
            if (dto.Attachment != null && dto.Attachment.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.Attachment.CopyToAsync(stream);

                dto.FileName = fileName;
                dto.FilePath = $"Uploads/Timesheets/{fileName}";
            }

            int id = await _timesheetService.SaveTimesheetAsync(dto);
            return Ok(new { message = "Timesheet saved successfully", timesheetId = id });
        }

        // ✅ USER LISTING
        [HttpGet("GetMyTimesheets/{userId}")]
        public async Task<IActionResult> GetMyTimesheets(int userId)
        {
            var data = await _timesheetService.GetMyTimesheetsAsync(userId);
            return Ok(data);
        }
        [HttpPost("SendSelectedTimesheets")]
        public async Task<IActionResult> SendSelectedTimesheets([FromBody] List<int> ids)
        {
            var result = await _timesheetService.SendSelectedTimesheetsAsync(ids);

            return Ok(new { success = true, message = "Timesheets submitted successfully" });
        }

        [HttpGet("GetManagerTimesheets/{managerUserId}")]
        public async Task<IActionResult> GetManagerTimesheets(int managerUserId)
        {
            var result = await _timesheetService.GetTimesheetsForManagerAsync(managerUserId);
            return Ok(result);
        }

        [HttpGet("GetTimesheetDetail/{timesheetId}")]
        public async Task<IActionResult> GetTimesheetDetail(int timesheetId)
        {
            var result = await _timesheetService.GetTimesheetDetailAsync(timesheetId);
            return Ok(result);
        }
        [HttpPost("ApproveTimesheets")]
        public async Task<IActionResult> ApproveTimesheets([FromBody] ApproveTimesheetRequestDto dto)
        {
            var result = await _timesheetService.ApproveTimesheetsAsync(dto.Ids, dto.Comments);
            return Ok(new { success = result });
        }

        [HttpPost("RejectTimesheets")]
        public async Task<IActionResult> RejectTimesheets([FromBody] ApproveTimesheetRequestDto dto)
        {
            var result = await _timesheetService.RejectTimesheetsAsync(dto.Ids, dto.Comments);
            return Ok(new { success = result });
        }

        #region missedpunchrequests
        [HttpPost("createmissedpunchrequest")]
        public async Task<IActionResult> CreateMissedPunchRequest(
        CreateMissedPunchRequestDto dto)
        {
            var result = await _service.CreateMissedPunchRequest(dto);
            return Ok(result);
        }

        [HttpGet("getmissedpunchrequest")]
        public async Task<IActionResult> GetMissedPunchRequest(
            int companyId, int? regionId)
        {
            return Ok(await _service.GetMissedPunchRequest(companyId, regionId));
        }

        [HttpGet("getapprovalmissedpunchrequest")]
        public async Task<IActionResult> GetApprovalMissedPunchRequest(
            int companyId, int? regionId, int managerId)
        {
            var result=await _service.GetApprovalMissedPunchRequest(
                companyId, regionId, managerId);
            return Ok(result);
        }

        [HttpPut("updatemissedpunch")]
        public async Task<IActionResult> UpdateMissedPunch(
            UpdateMissedPunchDto dto)
        {
            var success = await _service.UpdateMissedPunch(dto);
            if (!success)
                return BadRequest("Record not found or already processed.");

            return Ok(new { message = "Missed punch updated successfully" });
        }

        [HttpPost("bulkapproverejectpunch")]
        public async Task<IActionResult> BulkApproveRejectPunch(
            BulkApproveRejectPunchDto dto)
        {
            var count = await _service.BulkApproveRejectPunch(dto);

            if (count == 0)
                return BadRequest("No pending records found.");

            return Ok(new
            {
                message = $"{count} records {dto.Status.ToLower()} successfully"
            });
        }
        #endregion

        #region work from home requests
        // 🔹 CREATE WFH / REMOTE REQUEST
        [HttpPost("createWorkfromhome")]
        public async Task<IActionResult> Create(
            [FromBody] WfhRequestCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workfromhomeservice.CreateWorkFromHomeRequest(dto);
            return Ok(result);
        }

        // 🔹 EMPLOYEE – MY REQUESTS
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests(
            [FromQuery] int employeeId,
            [FromQuery] int companyId,
            [FromQuery] int? regionId)
        {
            var result = await _workfromhomeservice.GetMyWorkFromHomeRequests(
                employeeId, companyId, regionId);

            return Ok(result);
        }

        // 🔹 MANAGER – PENDING APPROVAL LIST
        [HttpGet("pending-approvals")]
        public async Task<IActionResult> GetPendingApprovals(
            [FromQuery] int companyId,
            [FromQuery] int? regionId,
            [FromQuery] int managerId)
        {
            var result = await _workfromhomeservice.GetPendingWorkFromHomeRequests(
                companyId, regionId, managerId);

            return Ok(result);
        }

        // 🔹 SINGLE APPROVE / REJECT
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(
            [FromBody] UpdateWorkFromHomeRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _workfromhomeservice.UpdateWorkFromHomeRequest(dto);

            if (!updated)
                return NotFound(new { message = "WFH request not found or already processed" });

            return Ok(new { message = "WFH request updated successfully" });
        }

        // 🔥 BULK APPROVE / REJECT
        [HttpPost("bulk-approve-reject")]
        public async Task<IActionResult> BulkApproveReject(
            [FromBody] BulkApproveRejectWorkFromHomeDto dto)
        {
            if (dto.WFHRequestIDs == null || !dto.WFHRequestIDs.Any())
                return BadRequest(new { message = "No WFH request IDs provided" });

            var count = await _workfromhomeservice.BulkApproveRejectWorkFromHome(dto);

            return Ok(new
            {
                message = $"Successfully updated {count} request(s)",
                updatedCount = count
            });
        }

        #endregion

        //----------------------------------------COMPANY NEWS---------------------------------------------//
        // ------------------------------------------------------
        // REGION: Add Company News (ADMIN ONLY)
        // ------------------------------------------------------
        #region Add Company News

        [HttpPost("add-news")]
        public async Task<IActionResult> AddCompanyNews([FromForm] CompanyNewsDto request)
        {
            if (request.CreatedBy <= 0)
                return BadRequest("CreatedBy (User ID) is required.");

            var success = await _companyNewsService.AddCompanyNewsAsync(request, request.CreatedBy);

            if (!success)
                return Forbid("Only admins can add company news.");

            return Ok("News Created Successfully");
        }




        // ------------------------------------------------------
        // REGION: Get All News (ANY EMPLOYEE)
        // ------------------------------------------------------
        // Filtered news endpoint
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] DateTime? date)
        {
            var result = await _companyNewsService.GetFilteredCompanyNewsAsync(category, date);
            return Ok(result);
        }



        // ------------------------------------------------------
        // REGION: Department Dropdown
        // ------------------------------------------------------
        // Department Dropdown
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await _companyNewsService.GetDepartmentsAsync();
            return Ok(result);
        }

        // ------------------------------------------------------
        // Update Company News (ADMIN ONLY)
        // ------------------------------------------------------
        [HttpPut("update-news")]
        public async Task<IActionResult> UpdateCompanyNews([FromForm] CompanyNewsDto request)
        {
            if (request.NewsId <= 0)
                return BadRequest("NewsId is required for update.");

            if (request.CreatedBy <= 0)
                return BadRequest("CreatedBy (User ID) is required.");

            var success = await _companyNewsService.UpdateCompanyNewsAsync(request, request.CreatedBy);

            if (!success)
                return Forbid("Only admins can update company news.");

            return Ok("News Updated Successfully");
        }

        // ------------------------------------------------------
        // Delete Company News (ADMIN ONLY)
        // ------------------------------------------------------
        [HttpDelete("delete-news/{id:int}")]
        public async Task<IActionResult> DeleteCompanyNews(int id, [FromQuery] int userId)
        {
            if (userId <= 0)
                return BadRequest("UserId is required.");

            var success = await _companyNewsService.DeleteCompanyNewsAsync(id, userId);

            if (!success)
                return Forbid("Only admins can delete company news.");

            return Ok("News Deleted Successfully");
        }


        #endregion
        //--------------------------company policy-------------------------------------//

        #region
        [HttpPost]
        [Route("GetAllPolicies")]
        public async Task<IActionResult> GetAllPolicies()
        {
            return Ok(await _companyPolicyService.GetAllPolicies());
        }

        [HttpPost]
        [Route("GetPolicyById")]
        public async Task<IActionResult> GetPolicyById([FromForm] int policyId)
        {
            var data = await _companyPolicyService.GetPolicyById(policyId);
            return Ok(data);
        }

        [HttpPost]
        [Route("GetPolicyCategoryDropdown")]
        public async Task<IActionResult> GetPolicyCategoryDropdown([FromForm] int companyId, [FromForm] int regionId)
        {
            var list = await _companyPolicyService.GetPolicyCategoryDropdown(companyId, regionId);
            return Ok(list);
        }

        [HttpPost]
        [Route("CreatePolicy")]
        public async Task<IActionResult> CreatePolicy([FromForm] CompanyPolicyDto dto)
        {
            return Ok(await _companyPolicyService.CreatePolicy(dto));
        }

        [HttpPost]
        [Route("UpdatePolicy")]
        public async Task<IActionResult> UpdatePolicy([FromForm] CompanyPolicyDto dto)
        {
            return Ok(await _companyPolicyService.UpdatePolicy(dto));
        }

        [HttpPost]
        [Route("DeletePolicy")]
        public async Task<IActionResult> DeletePolicy([FromForm] int policyId)
        {
            return Ok(await _companyPolicyService.DeletePolicy(policyId));
        }
        #endregion
    }
}
