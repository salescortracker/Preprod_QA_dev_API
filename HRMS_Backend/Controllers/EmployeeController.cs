using BusinessLayer.DTOs;
using BusinessLayer.Implementations;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeResignationService _resignationService;
        private readonly ILeaveService _leaveService;
        private readonly IemployeeService _employeeService;
        private readonly IShiftAllocationService _shiftAllocationService;
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeKpiService _kpiService;
        private readonly IManagerKpiReviewService _managerReviewService;
        private readonly IEmailService _emailService;
        private readonly HRMSContext _context;
        public EmployeeController(IEmployeeResignationService resignationService,IShiftAllocationService shiftAllocationService,  IemployeeService employeeService, ILeaveService leaveService, IWebHostEnvironment env, IEmployeeKpiService kpiService, IManagerKpiReviewService managerReviewService, IEmailService emailService, HRMSContext context)
        {
            _resignationService = resignationService;
            _employeeService = employeeService;
            _leaveService = leaveService;
            _shiftAllocationService = shiftAllocationService;
            _env = env;
            _emailService = emailService;
            _kpiService = kpiService;
            _context = context;
            _managerReviewService = managerReviewService;
        }

        #region Employee Resignation Details

        #region GetResignations
        /// <summary>
        /// Retrieves a list of all employee resignations.
        /// </summary>
        /// <remarks>
        /// Fetches all resignation records from the database using the service layer.
        /// </remarks>
        /// <returns>
        /// Returns HTTP 200 with list of resignation data.
        /// </returns>
        [HttpGet("GetResignations")]
        public async Task<IActionResult> GetResignations(int companyId, int regionId, int roleId)
        {
            var data = await _resignationService
                .GetResignationsByCompanyRegionAsync(companyId, regionId);

            return Ok(data);
        }
        #endregion

        #region GetResignationById
        /// <summary>
        /// Retrieves a resignation record by its unique identifier.
        /// </summary>
        /// <param name="id">Resignation ID to fetch.</param>
        /// <returns>
        /// HTTP 200 with resignation record, or 404 if not found.
        /// </returns>
        [HttpGet("GetResignationById")]
        public async Task<IActionResult> GetResignationById(int id, int companyId, int regionId)
        {
            var data = await _resignationService
                .GetResignationByIdFilteredAsync(id, companyId, regionId);

            if (data == null) return NotFound("Record not found or does not belong to this company/region.");

            return Ok(data);
        }
        #endregion

        #region GetResignationSearch
        /// <summary>
        /// Searches resignation records based on dynamic filter.
        /// </summary>
        /// <remarks>
        /// The filter object contains fields used to match records.
        /// </remarks>
        [HttpPost("GetResignationSearch")]
        public async Task<IActionResult> GetResignationSearch([FromBody] object filter)
        {
            var result = await _resignationService.SearchResignationsAsync(filter);
            return Ok(result);
        }
        #endregion

        #region SaveResignation
        /// <summary>
        /// Creates a new resignation entry.
        /// </summary>
        /// <param name="dto">Resignation DTO with employee data.</param>
        /// <returns>Returns created resignation record.</returns>
        [HttpPost("SaveResignation")]
        public async Task<IActionResult> SaveResignation([FromBody] EmployeeResignationDto dto)
        {
            var created = await _resignationService.AddResignationAsync(dto);
            return Ok(created);
        }
        #endregion

        #region UpdateResignation
        /// <summary>
        /// Updates an existing resignation entry.
        /// </summary>
        /// <param name="id">Resignation ID to update.</param>
        /// <param name="dto">Updated resignation details.</param>
        /// <returns>Returns the updated resignation DTO.</returns>
        [HttpPost("UpdateResignation/{id}")]
        public async Task<IActionResult> UpdateResignation(int id, [FromBody] EmployeeResignationDto dto)
        {
            var updated = await _resignationService.UpdateResignationAsync(id, dto);
            return Ok(updated);
        }
        #endregion

        #region DeleteResignation
        /// <summary>
        /// Deletes a resignation entry.
        /// </summary>
        /// <param name="id">Resignation ID to delete.</param>
        /// <returns>HTTP 204 No Content if deleted, 404 if not found.</returns>
        [HttpDelete("DeleteResignation/{id}")]
        public async Task<IActionResult> DeleteResignation(int id, int companyId, int regionId, int roleId)
        {
            var deleted = await _resignationService.DeleteResignationAsync(id, companyId, regionId);

            if (!deleted) return NotFound("Record not found or does not belong to this company/region.");

            return NoContent();
        }

        #endregion

       

public class UpdateResignationStatusRequest
{
            public int companyId { get; set; }
            public int regionId { get; set; }
            public int ResignationId { get; set; }
            public string Status { get; set; }
            public string? ManagerReason { get; set; }
            public bool IsManagerApprove { get; set; }
            public bool IsManagerReject { get; set; }
            // ✅ ADD THESE
            public string? HRReason { get; set; }
            public bool IsHRApprove { get; set; }
            public bool IsHRReject { get; set; }
        }

        #endregion
        #region Employee Job History,Education,Certification Details
        /// <summary>
        /// Retrieves all education records for employees.
        /// </summary>
        /// <remarks>This method fetches a list of all education details associated with employees. The
        /// data is retrieved asynchronously from the employee service.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing a collection of employee education records. The result is returned
        /// with an HTTP 200 OK status if successful.</returns>
        #region Employee Education
        [HttpGet("GetAllEducation")]
        public async Task<IActionResult> GetAllEducation()
        {
            var data = await _employeeService.getAllEmpEduAsync();
            return Ok(data);
        }
        /// <summary>
        /// Retrieves the education records associated with a specified user.
        /// </summary>
        /// <remarks>This method returns an HTTP 200 OK response with the user's education records. If no
        /// records are found, an empty list is returned instead of a 404 Not Found response.</remarks>
        /// <param name="userId">The unique identifier of the user whose education records are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> containing a list of <see cref="EmployeeEducationDto"/> objects representing
        /// the user's education records. Returns an empty list if no records are found.</returns>

        [HttpGet("user/{userId}/education")]
        public async Task<IActionResult> GetEducationByUserId(int userId)
        {
            var result = await _employeeService.getByUserIdempEduAsync(userId);

            // Return empty list instead of 404
            if (result == null || !result.Any())
                return Ok(new List<EmployeeEducationDto>());

            return Ok(result);
        }
        /// <summary>
        /// Retrieves the education record associated with the specified identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the education record from the
        /// data source.</remarks>
        /// <param name="id">The unique identifier of the education record to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the education record if found; otherwise, a NotFound result with
        /// an error message.</returns>
        [HttpGet("education/{id}")]
        public async Task<IActionResult> GetEducationById(int id)
        {
            var data = await _employeeService.getByIdEmpEduAsync(id);
            if (data == null)
                return NotFound(new { message = "Education not found" });

            return Ok(data);
        }
        /// <summary>
        /// Adds a new education record for an employee.
        /// </summary>
        /// <remarks>This method processes the provided education details and certificate file, saving the
        /// file to a designated directory. It then stores the education record in the database and returns the
        /// identifier of the newly created record.</remarks>
        /// <param name="model">The education details and certificate file to be added. The certificate file is optional but, if provided,
        /// must be non-empty.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns a success message and the ID
        /// of the new record if successful; otherwise, returns a bad request with validation errors.</returns>
        [HttpPost("AddEducation")]
        public async Task<IActionResult> AddEducation([FromForm] EmployeeEducationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string path = Path.Combine(root, "Uploads", "EmployeeEducationCertificates");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.CertificateFile != null && model.CertificateFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.CertificateFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.CertificateFile.CopyToAsync(stream);

                model.CertificateFilePath = $"Uploads/EmployeeEducationCertificates/{fileName}";
            }

            model.CreatedBy = model.UserId;

            var id = await _employeeService.addEmpEduAsync(model);

            return Ok(new { message = "Saved successfully", id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateEducation/{id}")]
        public async Task<IActionResult> UpdateEducation(int id, [FromForm] EmployeeEducationDto model)
        {
            if (id != model.EducationId)
                return BadRequest("Id mismatch");

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string path = Path.Combine(root, "Uploads", "EmployeeEducationCertificates");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.CertificateFile != null && model.CertificateFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.CertificateFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.CertificateFile.CopyToAsync(stream);

                model.CertificateFilePath = $"Uploads/EmployeeEducationCertificates/{fileName}";
            }

            model.CreatedBy = model.UserId;
            model.ModifiedBy = model.UserId;

            var result = await _employeeService.updateEmpEduAsync(model);
            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteEducation")]
        public async Task<IActionResult> DeleteEducation([FromQuery] int id)
        {
            var result = await _employeeService.deleteEmpEduAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("modeofstudy")]
        public async Task<IActionResult> GetModeOfStudy()
        {
            var data = await _employeeService.GetModeOfStudyListAsync();
            return Ok(data);
        }

        #endregion

        #region Employee Certifications

        /// <summary>
        /// Retrieves all employee certifications.
        /// </summary>
        /// <returns>A list of EmployeeCertificationDto objects.</returns>
        [HttpGet("certifications")]
        public async Task<IActionResult> GetAllCertifications()
        {
            var data = await _employeeService.getAllEmpCertAsync();
            return Ok(data);
        }

        /// <summary>
        /// Retrieves certifications for a specific user.
        /// </summary>
        /// <param name="userId">The user ID to filter certifications.</param>
        /// <returns>A list of EmployeeCertificationDto objects for the given user.</returns>
        [HttpGet("user/{userId}/certifications")]
        public async Task<IActionResult> GetUserCertifications(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdEmpCertAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No certifications found" });

            return Ok(data);
        }

        /// <summary>
        /// Retrieves a certification by its ID.
        /// </summary>
        /// <param name="id">The certification ID.</param>
        /// <returns>An EmployeeCertificationDto object if found.</returns>
        [HttpGet("certifications/{id}")]
        public async Task<IActionResult> GetCertificationById(int id)
        {
            var data = await _employeeService.getByIdEmpEduAsync(id);
            if (data == null)
                return NotFound(new { message = "Certification not found" });

            return Ok(data);
        }

        /// <summary>
        /// Adds a new employee certification.
        /// </summary>
        /// <param name="model">The EmployeeCertificationDto object containing certification details and file.</param>
        /// <returns>The ID of the newly created certification.</returns>
        [HttpPost("AddCertification")]
        public async Task<IActionResult> AddCertification([FromForm] EmployeeCertificationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeCertifications");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.DocumentFile != null && model.DocumentFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.DocumentFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.DocumentFile.CopyToAsync(stream);

                model.DocumentPath = $"Uploads/EmployeeCertifications/{fileName}";
            }
            model.CreatedBy = model.UserId;

            var id = await _employeeService.addEmpCertAsync(model);
            return Ok(new { message = "Saved successfully", id });
        }

        /// <summary>
        /// Updates an existing employee certification.
        /// </summary>
        /// <param name="id">The certification ID.</param>
        /// <param name="model">The EmployeeCertificationDto object containing updated details and file.</param>
        /// <returns>A success message if updated.</returns>
        [HttpPost("UpdateCertification/{id}")]
        public async Task<IActionResult> UpdateCertification(int id, [FromForm] EmployeeCertificationDto model)
        {
            if (id != model.CertificationId)
                return BadRequest("Id mismatch");

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeCertifications");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.DocumentFile != null && model.DocumentFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.DocumentFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.DocumentFile.CopyToAsync(stream);

                model.DocumentPath = $"Uploads/EmployeeCertifications/{fileName}";
            }

            model.ModifiedBy = model.UserId;

            var result = await _employeeService.updateEmpCertAsync(model);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }

        /// <summary>
        /// Deletes an employee certification by ID.
        /// </summary>
        /// <param name="id">The certification ID.</param>
        /// <returns>A success message if deleted.</returns>
        [HttpPost("DeleteCertification")]
        public async Task<IActionResult> DeleteCertification([FromQuery] int id)
        {
            var result = await _employeeService.deleteEmpCertAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }

        /// <summary>
        /// Retrieves all active certification types.
        /// </summary>
        /// <returns>A list of CertificationTypeDto objects.</returns>
        [HttpGet("certificationtypes")]
        public async Task<IActionResult> GetCertificationTypes()
        {
            var data = await _employeeService.GetCertificationTypesAsync();
            return Ok(data);
        }

        #endregion


        #region Employee Job History

        /// <summary>
        /// Get all employee job history records
        /// </summary>
        [HttpGet("jobhistory")]
        public async Task<IActionResult> GetAllJobHistory()
        {
            var data = await _employeeService.getAllempJobAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get job history records for a specific user
        /// </summary>
        [HttpGet("user/{userId}/jobhistory")]
        public async Task<IActionResult> GetUserJobHistory(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdEmpJobAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No job history found" });

            return Ok(data);
        }

        /// <summary>
        /// Get a single job history record by ID
        /// </summary>
        [HttpGet("jobhistory/{id}")]
        public async Task<IActionResult> GetJobHistoryById(int id)
        {
            var data = await _employeeService.getByIdEmpJobAsync(id);

            if (data == null)
                return NotFound(new { message = "Job history not found" });

            return Ok(data);
        }

        /// <summary>
        /// Add a new job history entry
        /// </summary>
        [HttpPost("AddJobHistory")]
        public async Task<IActionResult> AddJobHistory([FromForm] EmployeeJobHistoryDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeJobHistoryDocuments");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // File upload
            if (model.UploadDocument != null && model.UploadDocument.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.UploadDocument.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.UploadDocument.CopyToAsync(stream);

                model.UploadDocumentPath = $"Uploads/EmployeeJobHistoryDocuments/{fileName}";
            }

            model.CreatedBy = model.UserId;

            var id = await _employeeService.addEmpJobAsync(model);

            return Ok(new { message = "Saved successfully", id });
        }

        /// <summary>
        /// Update an existing job history entry
        /// </summary>
        [HttpPost("updatejobhistory/{id}")]
        public async Task<IActionResult> UpdateJobHistory(int id, [FromForm] EmployeeJobHistoryDto model)
        {
            if (id != model.Id)
                return BadRequest(new { message = "Id mismatch" });

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeJobHistoryDocuments");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // File upload logic
            if (model.UploadDocument != null && model.UploadDocument.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.UploadDocument.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.UploadDocument.CopyToAsync(stream);

                model.UploadDocumentPath = $"Uploads/EmployeeJobHistoryDocuments/{fileName}";
            }

            model.ModifiedBy = model.UserId;

            var result = await _employeeService.updateEmpJobAsync(model);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }

        /// <summary>
        /// Delete a job history record
        /// </summary>
        [HttpPost("deletejobhistory")]
        public async Task<IActionResult> DeleteJobHistory([FromQuery] int id)
        {
            var result = await _employeeService.deleteEmpJobAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }

        #endregion
        #endregion
        #region Immigration

        [HttpGet("GetImmigration")]
        public async Task<IActionResult> GetAllEmployeeImmigrations()
        {
            var result = await _employeeService.GetAllImmigrationAsync();
            return Ok(result);
        }

        [HttpGet("GetByIdImmigration/{id}")]
        public async Task<IActionResult> GetEmployeeImmigrationById(int id)
        {
            var result = await _employeeService.GetByIdImmigrationAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost("CreateImmigration")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEmployeeImmigration(
            [FromForm] EmployeeImmigrationDto dto,
            IFormFile? passportCopy,
            IFormFile? visaCopy,
            IFormFile? otherDocs)
        {
            try {
                var uploadPath = Path.Combine(_env.WebRootPath, "Uploads", "immigration");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                if (passportCopy != null)
                {
                    var fileName = Guid.NewGuid() + "_" + passportCopy.FileName;
                    var filePath = Path.Combine(uploadPath, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await passportCopy.CopyToAsync(stream);
                    dto.PassportCopyPath = fileName;
                }

                if (visaCopy != null)
                {
                    var fileName = Guid.NewGuid() + "_" + visaCopy.FileName;
                    var filePath = Path.Combine(uploadPath, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await visaCopy.CopyToAsync(stream);
                    dto.VisaCopyPath = fileName;
                }

                if (otherDocs != null)
                {
                    var fileName = Guid.NewGuid() + "_" + otherDocs.FileName;
                    var filePath = Path.Combine(uploadPath, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await otherDocs.CopyToAsync(stream);
                    dto.OtherDocumentsPath = fileName;
                }

                var success = await _employeeService.CreateImmigrationAsync(dto);
                return Ok(success);
            }
            catch(Exception ex)
            {
                throw ex;
            }
          }

        [HttpPost("UpdateImmigration/{id}")]
        public async Task<IActionResult> UpdateEmployeeImmigration(
            int id,
            [FromForm] EmployeeImmigrationDto dto,
            IFormFile? passportCopy,
            IFormFile? visaCopy,
            IFormFile? otherDocs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.ImmigrationId = id;

            var uploadPath = Path.Combine(_env.WebRootPath, "Uploads", "immigration");


            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // PASSPORT
            if (passportCopy != null)
            {
                var fileName = Guid.NewGuid() + "_" + passportCopy.FileName;
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await passportCopy.CopyToAsync(stream);
                }

                dto.PassportCopyPath = fileName;
            }

            // VISA
            if (visaCopy != null)
            {
                var fileName = Guid.NewGuid() + "_" + visaCopy.FileName;
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await visaCopy.CopyToAsync(stream);
                }

                dto.VisaCopyPath = fileName;
            }

            // OTHER
            if (otherDocs != null)
            {
                var fileName = Guid.NewGuid() + "_" + otherDocs.FileName;
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await otherDocs.CopyToAsync(stream);
                }

                dto.OtherDocumentsPath = fileName;
            }

            var success = await _employeeService.UpdateImmigrationAsync(dto);

            return success
                ? Ok(new { message = "Updated successfully" })
                : BadRequest(new { message = "Update failed" });
        }

        [HttpDelete("DeleteImmigration/{id}")]
        public async Task<IActionResult> DeleteEmployeeImmigration(int id)
        {
            var success = await _employeeService.DeleteImmigrationAsync(id);

            return success
                ? Ok(new { message = "Deleted successfully" })
                : BadRequest(new { message = "Delete failed" });
        }

        [HttpGet("DownloadImmigrationFile/{id}/{fileType}")]
        public async Task<IActionResult> DownloadImmigrationFile(int id, string fileType)
        {
            var data = await _employeeService.GetByIdImmigrationAsync(id);
            if (data == null)
                return NotFound(new { message = "Record not found" });

            string? fileName = fileType.ToLower() switch
            {
                "passport" => data.PassportCopyPath,
                "visa" => data.VisaCopyPath,
                "other" => data.OtherDocumentsPath,
                _ => null
            };

            if (string.IsNullOrEmpty(fileName))
                return NotFound(new { message = "File not found in database" });

            return Ok(new
            {
                filePath = $"Uploads/immigration/{fileName}"
            });
        }




        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }

        [HttpGet("GetVisaTypes")]
        public async Task<IActionResult> GetVisaTypes()
        {
            var list = await _employeeService.GetVisaTypesAsync();
            var response = list.
                Select(v => new
                {
                    visaTypeId = v.VisaTypeId,
                    visaTypeName = v.VisaTypeName
                })
                .ToList();

            return Ok(response);
        }
        [HttpGet("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var list = await _employeeService.GetStatusListAsync();
            var response = list.
        Select(s => new
        {
            statusId = s.StatusId,
            statusName = s.StatusName
        })
        .ToList();

            return Ok(response);
        }



        #endregion
        #region Employee Documents and Forms and Letters
        #region EmployeeDocuments
        [HttpGet("GetActiveDocumentTypes")]
        public async Task<IActionResult> GetActiveDocumentTypes()
        {
            var result = await _employeeService.GetActiveDocumentTypesAsync();
            return Ok(result);
        }
        [HttpGet("documents")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var data = await _employeeService.getAllempDocAsync();
            return Ok(data);
        }

        [HttpGet("user/{userId}/documents")]
        public async Task<IActionResult> GetDocumentsByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdempDocAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No records found" });

            return Ok(data);
        }

        [HttpGet("documents/{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var data = await _employeeService.getByIdempDocAsync(id);
            if (data == null)
                return NotFound(new { message = "Record not found" });

            return Ok(data);
        }

        [HttpPost("AddDocument")]
        public async Task<IActionResult> AddDocument([FromForm] EmployeeDocumentDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create folder
            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeDocuments");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Upload file
            if (model.DocumentFile != null && model.DocumentFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.DocumentFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.DocumentFile.CopyToAsync(stream);

                model.FileName = model.DocumentFile.FileName;
                model.FilePath = $"Uploads/EmployeeDocuments/{fileName}";
            }

            model.CreatedBy = model.UserId;

            int id = await _employeeService.addempDocAsync(model);
            return Ok(new { message = "Saved successfully", id });
        }

        [HttpPost("UpdateDocument/{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromForm] EmployeeDocumentDto model)
        {
            if (id != model.Id)
                return BadRequest("ID mismatch");

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeDocuments");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.DocumentFile != null && model.DocumentFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.DocumentFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.DocumentFile.CopyToAsync(stream);

                model.FileName = model.DocumentFile.FileName;
                model.FilePath = $"Uploads/EmployeeDocuments/{fileName}";
            }

            model.ModifiedBy = model.UserId;

            var result = await _employeeService.updateempDocAsync(model);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("DeleteDocument/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _employeeService.deleteempDocAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }


        #endregion


        #region Employee Forms

        [HttpGet("GetAllForms")]
        public async Task<IActionResult> GetAllForms()
        {
            var data = await _employeeService.getAllempFormAsync();
            return Ok(data);
        }

        [HttpGet("GetUserForms/{userId}")]
        public async Task<IActionResult> GetUserForms(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdempFormAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No forms found" });

            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetFormById/{id}")]
        public async Task<IActionResult> GetFormById(int id)
        {
            var data = await _employeeService.getByIdempFormAsync(id);
            if (data == null)
                return NotFound(new { message = "Form not found" });

            return Ok(data);
        }

        [HttpPost("AddForm")]
        public async Task<IActionResult> AddForm([FromForm] EmployeeFormDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "EmployeeForms");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.UploadFile != null && model.UploadFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.UploadFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.UploadFile.CopyToAsync(stream);

                model.FileName = fileName;
                model.FilePath = $"Uploads/EmployeeForms/{fileName}";
            }
            model.CreatedBy = model.UserId;

            var id = await _employeeService.addempFormAsync(model);
            return Ok(new { message = "Saved successfully", id });
        }

        [HttpPost("UpdateForm/{id}")]
        public async Task<IActionResult> UpdateForm(int id, [FromForm] EmployeeFormDto model)
        {
            if (id != model.Id)
                return BadRequest("Id mismatch");

            string root = _env.WebRootPath;
            string path = Path.Combine(root, "Uploads", "EmployeeForms");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (model.UploadFile != null && model.UploadFile.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{model.UploadFile.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.UploadFile.CopyToAsync(stream);

                model.FileName = fileName;
                model.FilePath = $"Uploads/EmployeeForms/{fileName}";
            }

            model.ModifiedBy = model.UserId;

            var result = await _employeeService.updateempFormAsync(model);
            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteForm")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var result = await _employeeService.deleteempFormAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }

        #endregion

        #region Employee Letters
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllLetters")]
        public async Task<IActionResult> GetAllLetters()
        {
            var data = await _employeeService.getAllempLetterAsync();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetLettersByUser/{userId}")]
        public async Task<IActionResult> GetLettersByUser(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdempLetterAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No letters found" });

            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetLetterById/{id}")]
        public async Task<IActionResult> GetLetterById(int id)
        {
            var data = await _employeeService.getByIdempLetterAsync(id);
            if (data == null)
                return NotFound(new { message = "Letter not found" });

            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost("AddLetter")]
        public async Task<IActionResult> AddLetter([FromForm] EmployeeLetterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string folder = Path.Combine(root, "Uploads", "EmployeeLetters");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (model.DocumentFile != null)
            {
                string fileName = $"{Guid.NewGuid()}_{model.DocumentFile.FileName}";
                string fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.DocumentFile.CopyToAsync(stream);

                model.FileName = fileName;
                model.FilePath = $"Uploads/EmployeeLetters/{fileName}";
            }

            model.CreatedBy = model.UserId;

            var id = await _employeeService.addempLetterAsync(model);
            return Ok(new { message = "Saved successfully", id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateLetter/{id}")]
        public async Task<IActionResult> UpdateLetter(int id, [FromForm] EmployeeLetterDto model)
        {
            if (id != model.Id)
                return BadRequest("Id mismatch");

            string root = _env.WebRootPath;
            string folder = Path.Combine(root, "Uploads", "EmployeeLetters");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (model.DocumentFile != null)
            {
                string fileName = $"{Guid.NewGuid()}_{model.DocumentFile.FileName}";
                string fullPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await model.DocumentFile.CopyToAsync(stream);

                model.FileName = fileName;
                model.FilePath = $"Uploads/EmployeeLetters/{fileName}";
            }

            model.ModifiedBy = model.UserId;

            var result = await _employeeService.updateempLetterAsync(model);

            if (!result)
                return NotFound(new { message = "Letter not found" });

            return Ok(new { message = "Updated successfully" });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("deleteletters")]
        public async Task<IActionResult> DeleteLetter([FromQuery] int id)
        {
            var result = await _employeeService.deleteempLetterAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }

        #endregion
        #endregion
        #region employee bank,dd,w4 details
        //-----------------------------------DROP-DOWN (ACCOUNT TYPE = EMPLOYEE.BANKDETAILS)----------------------------//
        //#region Account Type Details

        ///// <summary>
        ///// Get all active account types
        ///// </summary>
        //[HttpGet("GetActiveAccountTypes")]
        //public async Task<IActionResult> GetActiveAccountTypes()
        //{
        //    var result = await _accountTypeService.GetActiveAccountTypesAsync();
        //    return Ok(result);
        //}

        //#endregion

        ////----------------------------------DROP-DOWN (FILING STATUS = EMPLOYEE.BANKDETAILS)----------------------------------------//

        //[HttpGet("GetActiveFilingStatuses")]
        //public async Task<IActionResult> GetActiveFilingStatuses()
        //{
        //    var data = await _employeeFilingStatusService.GetActiveFilingStatusesAsync();
        //    return Ok(data);
        //}
        ////------------------------------DROPDOWN (STATES)------------------------------------------------------------------//
        //[HttpGet("GetActiveStates")]
        //public async Task<IActionResult> GetActiveStates()
        //{
        //    var data = await _employeeStateService.GetActiveStatesAsync();
        //    return Ok(data);
        //}

        //-----------------------------------Employee-Finance (BANK-DETAILS)--------------------------------------------//
        #region Employee Bank Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBankDetails")]
        public async Task<IActionResult> GetAllBankDetails()
        {
            var data = await _employeeService.getAllempBankAsync();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetBankDetailsById/{id}")]
        public async Task<IActionResult> GetBankDetailsById(int id)
        {
            var data = await _employeeService.getByIdempBankAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateBankDetails")]
        public async Task<IActionResult> CreateBankDetails([FromBody] EmployeeBankDetailsDto dto)
        {
            if (dto == null) return BadRequest("Invalid data");
            var success = await _employeeService.addempBankAsync(dto);
            if (!success) return BadRequest("Failed to create bank details");
            return Ok(dto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateBankDetails")]
        public async Task<IActionResult> UpdateBankDetails([FromBody] EmployeeBankDetailsDto dto)
        {
            if (dto == null) return BadRequest("Invalid data");
            var success = await _employeeService.updateempBankAsync(dto);
            if (!success) return BadRequest("Failed to update bank details");
            return Ok(dto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteBankDetails")]
        public async Task<IActionResult> DeleteBankDetails([FromQuery] int id)
        {
            var success = await _employeeService.deleteempBankAsync(id);
            if (!success) return NotFound("Bank details not found");
            return NoContent();
        }

        #endregion


        //----------------------------------Employee-Finance (DD-LIST)--------------------------------------------//

        #region DD List CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDdlist")]
        public async Task<IActionResult> GetAllDdlist()
        {
            var data = await _employeeService.getAllempDDAsync();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDdlistById/{id}")]
        public async Task<IActionResult> GetDdlistById(int id)
        {
            var data = await _employeeService.getByIdempDDAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateDdlist")]
        public async Task<IActionResult> CreateDdlist([FromBody] EmployeeDdlistDto dto)
        {
            var result = await _employeeService.addempDDAsync(dto);
            if (!result) return BadRequest("Failed to create DD List entry.");
            return Ok(dto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateDdlist")]
        public async Task<IActionResult> UpdateDdlist([FromBody] EmployeeDdlistDto dto)
        {
            var result = await _employeeService.updateempDDAsync(dto);
            if (!result) return BadRequest("Failed to update DD List entry.");
            return Ok(dto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteDdlist")]
        public async Task<IActionResult> DeleteDdlist([FromQuery] int id)
        {
            var result = await _employeeService.deleteempDDAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        #endregion

        #region DD Copy File Upload / Download
        [HttpPost("UploadDDCopy")]
        public async Task<IActionResult> UploadDDCopy(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "DDCopies");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                // Return detailed error in dev
                return StatusCode(500, ex.Message + " | " + ex.StackTrace);
            }
        }


        [HttpGet("DownloadDDCopy/{fileName}")]
        public IActionResult DownloadDDCopy(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("Invalid filename");

            fileName = Path.GetFileName(fileName); // sanitize
            var filePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "DDCopies", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            var contentType = fileName.EndsWith(".pdf") ? "application/pdf" :
                              fileName.EndsWith(".png") ? "image/png" :
                              fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") ? "image/jpeg" :
                              "application/octet-stream";

            return PhysicalFile(filePath, contentType, fileName);
        }

        #endregion

        //----------------------------------Employee-Finance (W4(USA))--------------------------------------------//
        #region Employee W4
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllW4s")]
        public async Task<IActionResult> GetAllW4s()
        {
            var data = await _employeeService.getAllempW4Async();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetW4ById/{id}")]
        public async Task<IActionResult> GetW4ById(int id)
        {
            var data = await _employeeService.getByIdempW4Async(id);
            if (data == null) return NotFound();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateW4")]
        public async Task<IActionResult> CreateW4([FromBody] EmployeeW4Dto dto)
        {
            var result = await _employeeService.addempW4Async(dto);
            if (!result) return BadRequest("Failed to create W4 entry.");
            return Ok(dto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateW4")]
        public async Task<IActionResult> UpdateW4([FromBody] EmployeeW4Dto dto)
        {
            var result = await _employeeService.updateempW4Async(dto);
            if (!result) return BadRequest("Failed to update W4 entry.");
            return Ok(dto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteW4")]
        public async Task<IActionResult> DeleteW4([FromQuery] int id)
        {
            var result = await _employeeService.deleteempW4Async(id);
            if (!result) return NotFound();
            return NoContent();
        }
        #endregion
        #endregion
        #region Personal details
        // GET: api/PersonalDetails
        [HttpGet("GetempPersonalAll")]
        public async Task<IActionResult> GetempPersonalAll()
        {
            var result = await _employeeService.GetAllPersonalEmailAsync();
            return Ok(result);
        }

        // GET: api/PersonalDetails/5
        [HttpGet("GetByIdPersonalEmailAsync/{id}")]
        public async Task<IActionResult> GetByIdPersonalEmailAsync(int id)
        {
            var result = await _employeeService.GetByIdPersonalEmailAsync(id);
            if (result == null)
                return NotFound("Record not found");

            return Ok(result);
        }

        // POST: api/PersonalDetails/search
        [HttpPost("SearchempProfileAsync")]
        public async Task<IActionResult> SearchempProfileAsync([FromBody] object filter)
        {
            var result = await _employeeService.SearchPersonalEmailAsync(filter);
            return Ok(result);
        }
        // ---------------------------------------------------------
        // GET PERSONAL DETAILS BY USER ID
        // ---------------------------------------------------------
        [HttpGet("GetByUserIdempProfile/{userId}")]
        public async Task<IActionResult> GetByUserIdempProfile(int userId)
        {
            var result = await _employeeService.GetByUserIdempProfileAsync(userId);

            if (result == null)
                return Ok(null);   // Returning null with 200 OK as you requested

            return Ok(result);
        }
        // POST: api/PersonalDetails
        [HttpPost("AddPersonalEmailAsync")]
        public async Task<IActionResult> AddPersonalEmailAsync([FromForm] PersonalDetailsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                string path = Path.Combine(root, "Uploads", "EmployeeProfileDetails");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (dto.profilePicture != null && dto.profilePicture.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}_{dto.profilePicture.FileName}";
                    string fullPath = Path.Combine(path, fileName);

                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await dto.profilePicture.CopyToAsync(stream);

                    dto.profilePicturePath = $"Uploads/EmployeeProfileDetails/{fileName}";
                }
                var result = await _employeeService.AddPersonalEmailAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/PersonalDetails
        [HttpPost("UpdateempPersonalAsync")]
        public async Task<IActionResult> UpdateempPersonalAsync([FromForm] PersonalDetailsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                string path = Path.Combine(root, "Uploads", "EmployeeProfileDetails");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                if (dto.profilePicture != null && dto.profilePicture.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid()}_{dto.profilePicture.FileName}";
                    string fullPath = Path.Combine(path, fileName);

                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await dto.profilePicture.CopyToAsync(stream);

                    dto.profilePicturePath = $"Uploads/EmployeeProfileDetails/{fileName}";
                }
                var result = await _employeeService.UpdateempPersonalAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/PersonalDetails/5
        [HttpDelete("DeleteEmpProfileAsync/{id}")]
        public async Task<IActionResult> DeleteEmpProfileAsync(int id)
        {
            var success = await _employeeService.DeletePersonalEmailAsync(id);

            if (!success)
                return NotFound("Record not found");

            return Ok(new { message = "Deleted successfully" });
        }
        #endregion
        #region DigitalCard
        [HttpGet("GetDigitalCard/{userId}")]
        public async Task<IActionResult> GetDigitalCard(int userId)
        {
            var result = await _employeeService.GetDigitalCardAsync(userId);
            if (result == null) return
                    NotFound("User Not Found");
            return Ok(result);
        }

        #endregion

        #region employee Profile
        [HttpGet("GetempProfile/{userId}")]
        public async Task<IActionResult> GetempProfile(int userId)
        {
            var data = await _employeeService.GetEmployeeProfileAsync(userId);

            if (data == null)
                return NotFound(new { message = "Employee profile not found" });

            return Ok(new
            {
                message = "Profile Loaded Successfully",
                data
            });
        }
        #endregion
        #region employee Family Details
        /// <summary>
        /// Get all employee family records
        /// </summary>
        [HttpGet("getAllempFamilyAsync")]
        public async Task<IActionResult> getAllempFamilyAsync()
        {
            var data = await _employeeService.getAllempFamilyAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get family records for a specific user
        /// </summary>
        [HttpGet("getByUserIdempFamilyAsync/{userId}")]
        public async Task<IActionResult> getByUserIdempFamilyAsync(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdempFamilyAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No family details found" });

            return Ok(data);
        }

        /// <summary>
        /// Get a single family record by ID
        /// </summary>
        [HttpGet("getByIdempFamilyAsync/{id}")]
        public async Task<IActionResult> getByIdempFamilyAsync(int id)
        {
            var data = await _employeeService.getByIdempFamilyAsync(id);

            if (data == null)
                return NotFound(new { message = "Family record not found" });

            return Ok(data);
        }

        /// <summary>
        /// Add a new family entry
        /// </summary>
        [HttpPost("addempFamilyAsync")]
        public async Task<IActionResult> addempFamilyAsync([FromForm] EmployeeFamilyDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // set created by (audit)
            model.CreatedBy = model.CreatedBy == 0 && model.UserId.HasValue ? model.UserId.Value : model.CreatedBy;

            var id = await _employeeService.addempFamilyAsync(model);

            return Ok(new { message = "Saved successfully", id });
        }

        /// <summary>
        /// Update an existing family entry
        /// </summary>
        [HttpPost("updateempFamilyAsync")]
        public async Task<IActionResult> updateempFamilyAsync([FromForm] EmployeeFamilyDto model)
        {
           
            model.ModifiedBy = model.ModifiedBy == 0 && model.UserId.HasValue ? model.UserId.Value : model.ModifiedBy;

            var result = await _employeeService.updateempFamilyAsync(model);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }

        /// <summary>
        /// Delete a family record
        /// </summary>
        [HttpDelete("deleteempFamilyAsync/{id}")]
        public async Task<IActionResult> deleteempFamilyAsync(int id)
        {
            var result = await _employeeService.deleteempFamilyAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }



        #endregion
        #region employee Emergency Contact Details
        #region Employee Emergency Contact Details

        /// <summary>
        /// Get all emergency contact records
        /// </summary>
        [HttpGet("GetAllempEmerAsync")]
        public async Task<IActionResult> GetAllempEmerAsync()
        {
            var data = await _employeeService.GetAllempEmerAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get emergency contacts for a specific user
        /// </summary>
        [HttpGet("GetByUserIdempEmerAsync/{userId}")]
        public async Task<IActionResult> GetByUserIdempEmerAsync(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.GetByUserIdempEmerAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No emergency contact details found" });

            return Ok(data);
        }

        /// <summary>
        /// Get a single emergency contact record by ID
        /// </summary>
        [HttpGet("GetByIdempEmerAsync/{id}")]
        public async Task<IActionResult> GetByIdempEmerAsync(int id)
        {
            var data = await _employeeService.GetByIdempEmerAsync(id);

            if (data == null)
                return NotFound(new { message = "Emergency contact record not found" });

            return Ok(data);
        }

        /// <summary>
        /// Add a new emergency contact entry
        /// </summary>
        [HttpPost("AddempEmerAsync")]
        public async Task<IActionResult> AddempEmerAsync([FromBody] EmployeeEmergencyContactDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Audit - createdBy
            if (string.IsNullOrEmpty(model.CreatedBy) && model.UserId != 0)
                model.CreatedBy = model.UserId.ToString();

            var id = await _employeeService.AddempEmerAsync(model);

            return Ok(new { message = "Saved successfully", id });
        }

        /// <summary>
        /// Update an existing emergency contact entry
        /// </summary>
        [HttpPost("UpdateempEmerAsync")]
        public async Task<IActionResult> UpdateempEmerAsync([FromBody] EmployeeEmergencyContactDto model)
        {
            if (model.EmergencyContactId != model.EmergencyContactId)
                return BadRequest(new { message = "Id mismatch" });

            // Audit - modifiedBy
            if (string.IsNullOrEmpty(model.ModifiedBy) && model.UserId != 0)
                model.ModifiedBy = model.UserId.ToString();

            var result = await _employeeService.UpdateempEmerAsync(model);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }

        /// <summary>
        /// Delete an emergency contact record
        /// </summary>
        [HttpPost("DeleteempEmerAsync")]
        public async Task<IActionResult> DeleteempEmerAsync([FromQuery] int id)
        {
            var result = await _employeeService.DeleteempEmerAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }

        /// <summary>
        /// Get relationship list for emergency contact dropdowns
        /// </summary>


        #endregion
        #endregion
        #region Employee References


        /// <summary>
        /// Get all employee references
        /// </summary>
        [HttpGet("getAllempRefAsync")]
        public async Task<IActionResult> getAllempRefAsync()
        {
            var data = await _employeeService.getAllempRefAsync();
            return Ok(data);
        }

        /// <summary>
        /// Get employee references for a specific user
        /// </summary>
        [HttpGet("getByUserIdempRefAsync/{userId}")]
        public async Task<IActionResult> getByUserIdempRefAsync(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid userId" });

            var data = await _employeeService.getByUserIdempRefAsync(userId);

            if (data == null || !data.Any())
                return NotFound(new { message = "No reference records found" });

            return Ok(data);
        }

        /// <summary>
        /// Get a single employee reference by ID
        /// </summary>
        [HttpGet("getByIdempRefAsync/{id}")]
        public async Task<IActionResult> getByIdempRefAsync(int id)
        {
            var data = await _employeeService.getByIdempRefAsync(id);

            if (data == null)
                return NotFound(new { message = "Reference record not found" });

            return Ok(data);
        }

        /// <summary>
        /// Create a new employee reference record
        /// </summary>
        [HttpPost("addempRefAsync")]
        public async Task<IActionResult> addempRefAsync([FromBody] EmployeeReferenceDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.CreatedBy = (model.CreatedBy == null || model.CreatedBy == 0)
                ? model.UserId
                : model.CreatedBy;

            var id = await _employeeService.addempRefAsync(model);

            return Ok(new { message = "Saved successfully", id });
        }

        /// <summary>
        /// Update an existing employee reference
        /// </summary>
        [HttpPost("updateempRefAsync")]
        public async Task<IActionResult> updateempRefAsync([FromBody] EmployeeReferenceDto model)
        {
            if (model.ReferenceId != model.ReferenceId)
                return BadRequest(new { message = "Id mismatch" });

            if (!ModelState.IsValid) return BadRequest(ModelState);

            model.ModifiedBy = (model.ModifiedBy == null || model.ModifiedBy == 0)
                ? model.UserId
                : model.ModifiedBy;

            var result = await _employeeService.updateempRefAsync(model);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Updated successfully" });
        }

        /// <summary>
        /// Delete employee reference record
        /// </summary>
        [HttpPost("deleteempRefAsync")]
        public async Task<IActionResult> deleteempRefAsync([FromQuery] int id)
        {
            var result = await _employeeService.deleteempRefAsync(id);

            if (!result)
                return NotFound(new { message = "Record not found" });

            return Ok(new { message = "Deleted successfully" });
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetReportingManager/{userId}")]
        public async Task<IActionResult> GetReportingManager(int userId)
        {
            var data = await _leaveService.GetReportingManagerAsync(userId);
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetActiveLeaveTypes")]
        public async Task<IActionResult> GetActiveLeaveTypes()
        {
            var data = await _leaveService.GetActiveLeaveTypesAsync();
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("SubmitLeave")]
        public async Task<IActionResult> SubmitLeave([FromForm] LeaveRequestDto dto)
        {
            string root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = Path.Combine(root, "Uploads", "LeaveDocuments");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // ✅ File upload
            if (dto.SupportingDocument != null && dto.SupportingDocument.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}_{dto.SupportingDocument.FileName}";
                string fullPath = Path.Combine(path, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.SupportingDocument.CopyToAsync(stream);

                dto.FileName = fileName;
                dto.FilePath = $"Uploads/LeaveDocuments/{fileName}";
            }

            // ✅ Save Leave
            int leaveId = await _leaveService.SubmitLeaveAsync(dto);

            // ✅ Send Email to Manager
            await _leaveService.SendLeaveEmailToManagerAsync(leaveId);

            return Ok(new { message = "Leave submitted and email sent successfully", id = leaveId });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetMyLeaves/{userId}")]
        public async Task<IActionResult> GetMyLeaves(int userId)
        {
            var data = await _leaveService.GetMyLeavesAsync(userId);
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaveId"></param>
        /// <returns></returns>
        [HttpGet("ApproveFromEmail/{leaveId}")]
        public async Task<IActionResult> ApproveFromEmail(int leaveId)
        {
            var result = await _leaveService.ApproveLeaveFromEmailAsync(leaveId);
            if (!result) return Content("Leave request not found");

            return Content("<h2>✅ Leave Approved Successfully</h2>", "text/html");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaveId"></param>
        /// <returns></returns>
        [HttpGet("RejectFromEmail/{leaveId}")]
        public async Task<IActionResult> RejectFromEmail(int leaveId)
        {
            var result = await _leaveService.RejectLeaveFromEmailAsync(leaveId);
            if (!result) return Content("Leave request not found");

            return Content("<h2>❌ Leave Rejected Successfully</h2>", "text/html");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        [HttpGet("GetLeavesForManager/{managerId}")]
        public async Task<IActionResult> GetLeavesForManager(int managerId)
        {
            var data = await _leaveService.GetLeavesForManagerAsync(managerId);
            return Ok(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaveId"></param>
        /// <returns></returns>
        [HttpPost("ApproveByManager/{leaveId}")]
        public async Task<IActionResult> ApproveByManager(int leaveId)
        {
            var result = await _leaveService.ApproveLeaveByManagerAsync(leaveId);
            return result ? Ok("Approved") : BadRequest("Failed");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leaveId"></param>
        /// <returns></returns>
        [HttpPost("RejectByManager/{leaveId}")]
        public async Task<IActionResult> RejectByManager(int leaveId)
        {
            var result = await _leaveService.RejectLeaveByManagerAsync(leaveId);
            return result ? Ok("Rejected") : BadRequest("Failed");
        }

        [HttpPost("BulkApprove")]
        public async Task<IActionResult> BulkApprove([FromBody] List<int> leaveIds)
        {
            var result = await _leaveService.BulkApproveLeavesAsync(leaveIds);
            return result ? Ok("Bulk Approved") : BadRequest();
        }

        [HttpPost("BulkReject")]
        public async Task<IActionResult> BulkReject([FromBody] List<int> leaveIds)
        {
            var result = await _leaveService.BulkRejectLeavesAsync(leaveIds);
            return result ? Ok("Bulk Rejected") : BadRequest();
        }
        [HttpGet("GetUserLeaves/{userId}")]
        public async Task<IActionResult> GetUserLeaves(int userId)
        {
            var result = await _leaveService.GetLeavesForUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("GetManagerLeaves/{managerId}")]
        public async Task<IActionResult> GetManagerLeaves(int managerId)
        {
            var result = await _leaveService.GetLeavesForManagerAsync(managerId);
            return Ok(result);
        }

        // ======================================================
        //                  GET ALL ALLOCATIONS
        // ======================================================
        [HttpGet("GetshiftAllocationAll")]
        public async Task<IActionResult> GetshiftAllocationAll()
        {
            var result = await _shiftAllocationService.GetAllAllocationsAsync();
            return Ok(result);
        }

        // ======================================================
        //                  GET BY ID
        // ======================================================
        [HttpGet("GetshiftAllocationById/{id}")]
        public async Task<IActionResult> GetshiftAllocationById(int id)
        {
            var allocation = await _shiftAllocationService.GetAllocationByIdAsync(id);

            if (allocation == null)
                return NotFound(new { message = "Shift allocation not found" });

            return Ok(allocation);
        }

        // ======================================================
        //                  ALLOCATE SHIFT
        // ======================================================
        [HttpPost("AddAllocateShift")]
        public async Task<IActionResult> AddAllocateShift([FromBody] ShiftAllocationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _shiftAllocationService.AllocateShiftAsync(dto);

            if (!success)
                return BadRequest(new { message = "Failed to allocate shift" });

            return Ok(new { message = "Shift allocated successfully" });
        }

        // ======================================================
        //                  UPDATE SHIFT ALLOCATION
        // ======================================================
        [HttpPost("UpdateAllocation")]
        public async Task<IActionResult> UpdateAllocation([FromBody] ShiftAllocationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _shiftAllocationService.UpdateAllocationAsync(dto);

            if (!success)
                return NotFound(new { message = "Shift allocation not found" });

            return Ok(new { message = "Shift allocation updated successfully" });
        }

        // ======================================================
        //                  DELETE SHIFT ALLOCATION
        // ======================================================
        [HttpPost("DeleteAllocation")]
        public async Task<IActionResult> DeleteAllocation([FromQuery] int id)
        {
            var success = await _shiftAllocationService.DeleteAllocationAsync(id);

            if (!success)
                return NotFound(new { message = "Shift allocation not found" });

            return Ok(new { message = "Shift allocation deleted successfully" });
        }

        #region KPI Employee Operations
        /// <summary>
        /// Retrieves all employee KPIs.
        /// </summary>
        /// <returns>List of all KPIs.</returns>
        [HttpGet("GetAllKpis")]
        public async Task<IActionResult> GetAllKpis()
        {
            var kpis = await _kpiService.GetAllKpisAsync();
            return Ok(kpis);
        }

        /// <summary>
        /// Retrieves all KPIs for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>List of KPIs for the given user.</returns>
        [HttpGet("GetKpisByUser/{userId}")]
        public async Task<IActionResult> GetKpisByUser(int userId)
        {
            var kpis = await _kpiService.GetKpisByUserAsync(userId);
            return Ok(kpis);
        }

        /// <summary>
        /// Creates a new KPI entry for an employee.
        /// </summary>
        /// <param name="dto">EmployeeKpiDto containing KPI data and optional file upload.</param>
        /// <returns>ID of the created KPI and success message.</returns>
        [HttpPost("CreateKpi")]
        public async Task<IActionResult> CreateKpi([FromForm] EmployeeKpiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Required fields check
            if (dto.UserId <= 0 || dto.CompanyId <= 0 || dto.RegionId <= 0)
                return BadRequest("UserId, CompanyId, and RegionId are required.");

            // FILE UPLOAD
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadFolder = Path.Combine(root, "Uploads", "EmployeeKpiDocuments");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var fullPath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                dto.DocumentEvidencePath = Path.Combine("Uploads", "EmployeeKpiDocuments", fileName)
                                                        .Replace("\\", "/");
            }

            // SAVE KPI
            dto.CreatedBy = dto.UserId;
            dto.CreatedAt = DateTime.Now;

            var kpiId = await _kpiService.CreateKpiAsync(dto);

            // =================================================
            //      🚀 SEND EMAIL TO REPORTING MANAGER
            // =================================================

            // 1. Fetch manager email from Users table
            var manager = await _context.Users
                .Where(u => u.UserId == dto.ReportingManagerId)
                .FirstOrDefaultAsync();

            string? managerEmail = manager?.Email;

            if (string.IsNullOrEmpty(managerEmail))
            {
                return Ok(new
                {
                    Message = "KPI created but manager email not found.",
                    KpiId = kpiId
                });
            }

            // 2. Prepare email body
            string subject = $"New KPI Submitted by {dto.EmployeeNameId}";
            string body = $@"
        <h2>KPI Submitted</h2>
        <p><b>Employee:</b> {dto.EmployeeNameId}</p>
        <p><b>Appraisal Year:</b> {dto.AppraisalYear}</p>
        <p><b>Reporting Manager ID:</b> {dto.ReportingManagerId}</p>
        <p>Please log in to HRMS Manager Dashboard to review the submission.</p>
    ";

            try
            {
                await _emailService.SendEmailAsync(managerEmail, subject, body);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Message = "KPI created but failed to send email to manager.",
                    Error = ex.Message,
                    KpiId = kpiId
                });
            }

            return Ok(new { Message = "KPI created successfully & Email sent to Reporting Manager", KpiId = kpiId });
        }

        /// <summary>
        /// Updates an existing KPI entry for an employee.
        /// </summary>
        /// <param name="dto">EmployeeKpiDto containing updated KPI data and optional file upload.</param>
        /// <returns>Success or not found message.</returns>
        [HttpPost("UpdateKpi")]
        public async Task<IActionResult> UpdateKpi([FromForm] EmployeeKpiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Handle file upload
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadFolder = Path.Combine(root, "Uploads", "EmployeeKpiDocuments");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                string fullPath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Save relative path
                dto.DocumentEvidencePath = Path.Combine("Uploads", "EmployeeKpiDocuments", fileName)
                    .Replace("\\", "/");
            }

            // Set auditing
            dto.ModifiedBy = dto.UserId;
            dto.ModifiedAt = DateTime.Now;

            var isUpdated = await _kpiService.UpdateKpiAsync(dto);
            if (!isUpdated)
                return NotFound(new { Message = "KPI not found" });

            return Ok(new { Message = "KPI updated successfully" });
        }

        /// <summary>
        /// Deletes a KPI entry by its ID.
        /// </summary>
        /// <param name="kpiId">ID of the KPI to delete.</param>
        /// <returns>No content if deleted, or not found message.</returns>
        [HttpDelete("DeleteKpi/{kpiId}")]
        public async Task<IActionResult> DeleteKpi(int kpiId)
        {
            var isDeleted = await _kpiService.DeleteKpiAsync(kpiId);

            if (!isDeleted)
                return NotFound(new { Message = "KPI not found" });

            return NoContent();
        }

        #endregion


        #region Manager KPI Review Operations

        // ===================== MANAGER KPI REVIEW =====================

        [HttpGet("Manager/GetReviews/{managerId}")]
        public async Task<IActionResult> GetManagerReviews(int managerId)
        {
            var reviews = await _managerReviewService.GetAllPendingReviewsAsync(managerId);
            return Ok(reviews);
        }

        [HttpPut("Manager/UpdateReview")]
        public async Task<IActionResult> UpdateManagerReview([FromBody] ManagerReviewUpdateRequest request)
        {
            var updated = await _managerReviewService.UpdateReviewAsync(request);
            if (!updated)
                return NotFound(new { Message = "Review not found" });

            return Ok(new { Message = "Review updated successfully" });
        }

        [HttpPut("Manager/UpdateStatus")]
        public async Task<IActionResult> UpdateManagerReviewStatus([FromBody] ManagerReviewStatusUpdateRequest request)
        {
            var updated = await _managerReviewService.UpdateStatusAsync(request);
            if (!updated)
                return BadRequest(new { Message = "Failed to update status" });

            foreach (var reviewId in request.ReviewIds)
            {
                var review = await _managerReviewService.GetReviewByIdAsync(reviewId);

                if (review != null && !string.IsNullOrEmpty(review.EmployeeEmail))
                {
                    string subject = $"KPI Review {request.Status}";
                    string body = $"Dear {review.EmployeeName},<br/><br/>" +
                                  $"Your KPI for {review.AppraisalYear} is <b>{request.Status}</b>.<br/>" +
                                  $"Comments: {review.ManagerComments}<br/><br/>" +
                                  "Regards,<br/>HRMS Team";

                    await _emailService.SendEmailAsync(review.EmployeeEmail, subject, body);
                }
            }

            return Ok(new { Message = $"Status updated to {request.Status}" });
        }

        #endregion

        [HttpGet("GetResignationsForManager")]
        public async Task<IActionResult> GetResignationsForManager(int managerUserId)
        {
            var data = await _resignationService
                .GetResignationsForReportingManagerAsync(managerUserId);

            return Ok(data);
        }



        [HttpPut("UpdateResignationStatus")]
        public async Task<IActionResult> UpdateResignationStatus([FromBody] UpdateResignationStatusRequest request)
        {
            var result = await _resignationService.UpdateResignationStatusAsync(
                request.ResignationId,
                request.Status,
                request.ManagerReason,
                request.IsManagerApprove,
                request.IsManagerReject,
                request.HRReason,
    request.IsHRApprove,
    request.IsHRReject);

            if (!result)
                return NotFound("Invalid resignation");

            return Ok(new
            {
                success = true,
                status = request.Status
            });

        }
        [HttpGet("GetResignationsForHR")]
        public async Task<IActionResult> GetResignationsForHR(int companyId, int regionId)
        {
            var data = await _resignationService
                .GetResignationsForHRAsync(companyId, regionId);

            return Ok(data);
        }

    }
}
