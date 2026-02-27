using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpdeskController : ControllerBase
    {
        private readonly IHelpdeskService _helpdeskservice;
        public HelpdeskController(IHelpdeskService helpdeskservice)
        {
            _helpdeskservice = helpdeskservice;
        }
        [HttpGet]
        [Route("GetActivePriorities/{companyId}/{regionId}")]
        public async Task<IActionResult> GetActivePriorities(int companyId, int regionId)
        {
            var data = await _helpdeskservice.GetActivePrioritiesAsync(companyId, regionId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetActiveCategory/{companyId}/{regionId}")]
        public async Task<IActionResult> GetActivecategory(int companyId, int regionId)
        {
            var data = await _helpdeskservice.GetActivecategoryAsync(companyId, regionId);
            return Ok(data);
        }
        [HttpGet]
        [Route("GetUserProfile/{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var data = await _helpdeskservice.GetUserProfileAsync(userId);
            return Ok(data);
        }
        [HttpPost("SubmitTicket")]
        public async Task<IActionResult> SubmitTicket([FromForm] TicketRequestDto dto)
        {
            string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "Helpdesk");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (dto.Attachment != null && dto.Attachment.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.Attachment.CopyToAsync(stream);

                dto.FileName = fileName;
                dto.FilePath = $"Uploads/Helpdesk/{fileName}";
            }

            // ✅ Save Ticket
            int ticketId = await _helpdeskservice.SubmitTicketAsync(dto);

            // ✅ Send Email to Manager
            await _helpdeskservice.SendTicketEmailToManagerAsync(ticketId);

            return Ok(new { message = "Ticket submitted successfully", id = ticketId });
        }

        // ✅ MY TICKETS
        [HttpGet("GetMyTickets/{userId}")]
        public async Task<IActionResult> GetMyTickets(int userId)
        {
            var data = await _helpdeskservice.GetMyTicketsAsync(userId);
            return Ok(data);
        }
        // ✅ MANAGER SCREEN LISTING
        [HttpGet("GetManagerTickets/{managerId}")]
        public async Task<IActionResult> GetManagerTickets(int managerId)
        {
            var data = await _helpdeskservice.GetManagerTicketsAsync(managerId);
            return Ok(data);
        }

        // ✅ APPROVE / REJECT
        [HttpPost("UpdateTicketStatus")]
        public async Task<IActionResult> UpdateTicketStatus([FromBody] UpdateTicketStatusDto dto)
        {
            await _helpdeskservice.UpdateTicketStatusAsync(dto);
            return Ok(new { message = "Ticket updated successfully" });
        }

        [HttpGet("GetEmployeesByManager/{managerId}")]
        public async Task<IActionResult> GetEmployeesByManager(int managerId)
        {
            var data = await _helpdeskservice.GetEmployeesByManagerAsync(managerId);
            return Ok(data);
        }
    }
}
