using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _service;
        public RecruitmentController(IRecruitmentService service)
        {
            _service = service;

        }
        [HttpPost("SaveCandidate")]
        public async Task<IActionResult> SaveCandidate([FromForm] CandidateDto dto)
        {
            string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "Resumes");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (dto.ResumeFile != null && dto.ResumeFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{dto.ResumeFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.ResumeFile.CopyToAsync(stream);

                dto.FileName = fileName;
                dto.FilePath = $"Uploads/Resumes/{fileName}";
            }

            int id = await _service.SaveCandidateAsync(dto);
            return Ok(new { message = "Candidate saved successfully", candidateId = id });
        }

        // 🔹 GET CANDIDATES
        [HttpGet("GetCandidates/{userId}/{companyId}/{regionId}")]

        public async Task<IActionResult> GetCandidates(int userId, int companyId, int regionId)
        {
            var data = await _service.GetCandidatesAsync(userId, companyId, regionId);
            return Ok(data);
        }

        // 🔹 MOVE STAGE
        [HttpPost("MoveStage")]
        public async Task<IActionResult> MoveStage(int candidateId, int stageId)
        {
            bool success = await _service.MoveStageAsync(candidateId, stageId);
            return success ? Ok() : BadRequest();
        }

        // 🔹 DELETE
        [HttpDelete("DeleteCandidate/{candidateId}")]
        public async Task<IActionResult> DeleteCandidate(int candidateId)
        {
            bool success = await _service.DeleteCandidateAsync(candidateId);
            return success ? Ok() : BadRequest();
        }

        [HttpGet("GetCandidateById/{candidateId}")]
        public async Task<IActionResult> GetCandidateById(int candidateId)
        {
            var data = await _service.GetCandidateByIdAsync(candidateId);
            return data == null ? NotFound() : Ok(data);
        }
        [HttpPut("UpdateCandidate")]
        public async Task<IActionResult> UpdateCandidate([FromForm] CandidateDto dto)
        {
            var success = await _service.UpdateCandidateAsync(dto);
            return success ? Ok() : BadRequest();
        }


        [HttpGet("GetReferenceUsers/{companyId}/{regionId}")]
        public async Task<IActionResult> GetReferenceUsers(int companyId, int regionId)
        {
            var data = await _service.GetReferenceUsersAsync(companyId, regionId);
            return Ok(data);
        }

        ///////////Screening////////

        [HttpGet("GetRecruiters/{companyId}/{regionId}")]
        public async Task<IActionResult> GetRecruiters(int companyId, int regionId)
        {
            var data = await _service.GetRecruitersAsync(companyId, regionId);
            return Ok(data);
        }
        [HttpGet("GetScreeningCandidatesTopTable")]
        public async Task<IActionResult> GetScreeningCandidatesTopTable(
int companyId,
int regionId,
string department,
string designation)
        {
            var result = await _service
                .GetScreeningCandidatesTopTableAsync(companyId, regionId, department, designation);

            return Ok(result);
        }


        [HttpPost("SaveScreening")]
        public async Task<IActionResult> SaveScreening(
[FromBody] CandidateScreeningDto dto)
        {
            var result = await _service.SaveCandidateScreeningAsync(dto);

            if (!result)
                return BadRequest("Unable to save screening");

            return Ok(new { message = "Candidate moved to Interview stage" });
        }
        [HttpGet("GetScreeningRecords/{userId}/{companyId}/{regionId}")]
        public async Task<IActionResult> GetScreeningRecords(int userId, int companyId, int regionId)
        {
            var data = await _service.GetScreeningRecordsAsync(userId, companyId, regionId);
            return Ok(data);
        }
        [HttpPut("UpdateScreening")]
        public async Task<IActionResult> UpdateScreening([FromBody] CandidateScreeningDto dto)
        {
            var result = await _service.UpdateCandidateScreeningAsync(dto);
            return result ? Ok() : BadRequest("Unable to update screening");
        }


        ////////////Interview
        [HttpGet("GetScreeningCandidatesTopTableInterview")]
        public async Task<IActionResult> GetScreeningCandidatesTopTableInterview(
int companyId,
int regionId,
string department,
string designation)
        {
            var result = await _service
                .GetScreeningCandidatesTopTableInterviewAsync(companyId, regionId, department, designation);

            return Ok(result);
        }
        [HttpPost("SaveCandidateInterview")]
        public async Task<IActionResult> SaveCandidateInterview(
     [FromBody] CandidateInterviewDto dto)
        {
            var result = await _service.SaveCandidateInterviewAsync(dto);

            if (!result)
                return BadRequest("Unable to save interview");

            return Ok(new { message = "Interview scheduled successfully" });
        }
        [HttpGet("GetInterviewRecords/{userId}/{companyId}/{regionId}")]
        public async Task<IActionResult> GetInterviewRecords(int userId,
    int companyId,
    int regionId)
        {
            var data = await _service.GetInterviewRecordsAsync(userId, companyId, regionId);
            return Ok(data);
        }
        [HttpPut("UpdateCandidateInterview")]
        public async Task<IActionResult> UpdateCandidateInterview(
    [FromBody] CandidateInterviewDto dto)
        {
            var result = await _service.UpdateCandidateInterviewAsync(dto);

            if (!result)
                return BadRequest("Unable to update interview");

            return Ok(new { message = "Interview updated successfully" });
        }

        [HttpGet("GetAppointments/{companyId}/{regionId}/{interviewerId}")]
        public async Task<IActionResult> GetAppointments(
    int companyId,
    int regionId,
    int interviewerId)
        {
            var data = await _service.GetAppointmentsForInterviewerAsync(
                companyId,
                regionId,
                interviewerId
            );

            return Ok(data);
        }
        [HttpGet("GetAppointmentCandidateDetails/{candidateId}")]
        public async Task<IActionResult> GetAppointmentCandidateDetails(int candidateId)
        {
            var data = await _service.GetAppointmentCandidateDetailsAsync(candidateId);
            return data == null ? NotFound() : Ok(data);
        }
    }
}
