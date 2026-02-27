using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;
        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
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

    }
}
