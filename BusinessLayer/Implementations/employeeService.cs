using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class employeeService : IemployeeService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public employeeService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        #region employee Certification Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeCertificationDto>> getAllEmpCertAsync()
        {
            return await _context.EmployeeCertifications
                .Select(x => new EmployeeCertificationDto
                {
                    CertificationId = x.CertificationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,

                    CertificationName = x.CertificationName,
                    CertificationTypeId = x.CertificationTypeId,
                    Description = x.Description,
                    DocumentPath = x.DocumentPath,

                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate

                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeCertificationDto>> getByUserIdEmpCertAsync(int userId)
        {
            return await _context.EmployeeCertifications
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeCertificationDto
                {
                    CertificationId = x.CertificationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,
                    CertificationName = x.CertificationName,
                    CertificationTypeId = x.CertificationTypeId,
                    Description = x.Description,
                    DocumentPath = x.DocumentPath,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,

                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificationId"></param>
        /// <returns></returns>
        public async Task<EmployeeCertificationDto?> getByIdEmpCertAsync(int certificationId)
        {
            return await _context.EmployeeCertifications
                .Where(x => x.CertificationId == certificationId)
                .Select(x => new EmployeeCertificationDto
                {
                    CertificationId = x.CertificationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,
                    CertificationName = x.CertificationName,
                    CertificationTypeId = x.CertificationTypeId,
                    Description = x.Description,
                    DocumentPath = x.DocumentPath,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,

                })
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> addEmpCertAsync(EmployeeCertificationDto model)
        {
            var entity = new EmployeeCertification
            {
                CompanyId = model.CompanyId,
                RegionId = model.RegionId,
                UserId = model.UserId,
                CertificationName = model.CertificationName,
                CertificationTypeId = model.CertificationTypeId,
                Description = model.Description,
                DocumentPath = model.DocumentPath,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _context.EmployeeCertifications.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.CertificationId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> updateEmpCertAsync(EmployeeCertificationDto model)
        {
            var entity = await _context.EmployeeCertifications.FindAsync(model.CertificationId);

            if (entity == null) return false;

            entity.CertificationName = model.CertificationName;
            entity.CertificationTypeId = model.CertificationTypeId;
            entity.Description = model.Description;
            entity.DocumentPath = model.DocumentPath ?? entity.DocumentPath;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificationId"></param>
        /// <returns></returns>
        public async Task<bool> deleteEmpCertAsync(int certificationId)
        {
            var entity = await _context.EmployeeCertifications.FirstOrDefaultAsync(x => x.CertificationId == certificationId);

            if (entity == null) return false;

            _context.EmployeeCertifications.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CertificationTypeDto>> GetCertificationTypesAsync()
        {
            return await _context.CertificationTypes
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .Select(x => new CertificationTypeDto
                {
                    CertificationTypeID = x.CertificationTypeId,
                    CertificationTypeName = x.CertificationTypeName
                })
                .OrderBy(x => x.CertificationTypeName)
                .ToListAsync();
        }
        #endregion
        #region employee Education Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeEducationDto>> getAllEmpEduAsync()
        {
            return await _context.EmployeeEducations
                .Select(x => new EmployeeEducationDto
                {
                    EducationId = x.EducationId,
                    UserId = x.UserId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    ModeOfStudyId = x.ModeOfStudyId,
                    Qualification = x.Qualification,
                    Specialization = x.Specialization,
                    Institution = x.Institution,
                    Board = x.Board,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Result = x.Result,
                    CertificateFilePath = x.CertificateFilePath,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeEducationDto>> getByUserIdempEduAsync(int userId)
        {
            return await _context.EmployeeEducations
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeEducationDto
                {
                    EducationId = x.EducationId,
                    UserId = x.UserId,
                    Qualification = x.Qualification,
                    Specialization = x.Specialization,
                    Institution = x.Institution,
                    Board = x.Board,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Result = x.Result,
                    CertificateFilePath = x.CertificateFilePath,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    ModeOfStudyId = x.ModeOfStudyId
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="educationId"></param>
        /// <returns></returns>
        public async Task<EmployeeEducationDto?> getByIdEmpEduAsync(int educationId)
        {
            return await _context.EmployeeEducations
                .Where(x => x.EducationId == educationId)
                .Select(x => new EmployeeEducationDto
                {
                    EducationId = x.EducationId,
                    UserId = x.UserId,
                    Qualification = x.Qualification,
                    Specialization = x.Specialization,
                    Institution = x.Institution,
                    Board = x.Board,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Result = x.Result,
                    CertificateFilePath = x.CertificateFilePath,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    ModeOfStudyId = x.ModeOfStudyId
                })
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> addEmpEduAsync(EmployeeEducationDto model)
        {
            var entity = new EmployeeEducation
            {
                UserId = model.UserId,
                CompanyId = model.CompanyId,
                RegionId = model.RegionId,
                ModeOfStudyId = model.ModeOfStudyId,
                Qualification = model.Qualification,
                Specialization = model.Specialization,
                Institution = model.Institution,
                Board = model.Board,
                StartDate = model.StartDate ?? default,
                EndDate = model.EndDate ?? default,
                Result = model.Result,
                CertificateFilePath = model.CertificateFilePath,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };

            await _context.EmployeeEducations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.EducationId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> updateEmpEduAsync(EmployeeEducationDto model)
        {
            var entity = await _context.EmployeeEducations.FindAsync(model.EducationId);
            if (entity == null)
                return false;

            entity.UserId = model.UserId;
            entity.CompanyId = model.CompanyId;
            entity.RegionId = model.RegionId;
            entity.ModeOfStudyId = model.ModeOfStudyId;
            entity.Qualification = model.Qualification;
            entity.Specialization = model.Specialization;
            entity.Institution = model.Institution;
            entity.Board = model.Board;
            entity.StartDate = model.StartDate ?? entity.StartDate;
            entity.EndDate = model.EndDate ?? entity.EndDate;
            entity.Result = model.Result ?? entity.Result;
            entity.CertificateFilePath = model.CertificateFilePath ?? entity.CertificateFilePath;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;

            _context.EmployeeEducations.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="educationId"></param>
        /// <returns></returns>
        public async Task<bool> deleteEmpEduAsync(int educationId)
        {
            var entity = await _context.EmployeeEducations
                .FirstOrDefaultAsync(x => x.EducationId == educationId);

            if (entity == null)
                return false;

            _context.EmployeeEducations.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ModeOfStudyDto>> GetModeOfStudyListAsync()
        {
            return await _context.ModeOfStudies
                .Where(x => x.IsActive)
                .Select(x => new ModeOfStudyDto
                {
                    ModeOfStudyId = x.ModeOfStudyId,
                    ModeName = x.ModeName
                })
                .OrderBy(x => x.ModeName)
                .ToListAsync();
        }
        #endregion
        #region employee Job History Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeJobHistoryDto>> getAllempJobAsync()
        {
            return await _context.EmployeeJobHistories
                .Select(x => new EmployeeJobHistoryDto
                {
                    Id = x.Id,
                    Employer = x.Employer,
                    JobTitle = x.JobTitle,

                    // DateOnly → DateTime
                    FromDate = x.FromDate.ToDateTime(TimeOnly.MinValue),
                    ToDate = x.ToDate.ToDateTime(TimeOnly.MinValue),

                    LastCTC = x.LastCtc,
                    Website = x.Website,
                    EmployeeCode = x.EmployeeCode,
                    ReasonForLeaving = x.ReasonForLeaving,
                    UploadDocumentPath = x.UploadDocument,

                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,

                    CreatedBy = x.CreatedBy ?? 0,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// Asynchronously retrieves the job history for a specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose job history is to be retrieved. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
        /// cref="EmployeeJobHistoryDto"/> objects representing the user's job history, ordered by the creation date in
        /// descending order. The collection will be empty if no job history is found for the specified user.</returns>
        public async Task<IEnumerable<EmployeeJobHistoryDto>> getByUserIdEmpJobAsync(int userId)
        {
            return await _context.EmployeeJobHistories
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeJobHistoryDto
                {
                    Id = x.Id,
                    Employer = x.Employer,
                    JobTitle = x.JobTitle,

                    FromDate = x.FromDate.ToDateTime(TimeOnly.MinValue),
                    ToDate = x.ToDate.ToDateTime(TimeOnly.MinValue),

                    LastCTC = x.LastCtc,
                    Website = x.Website,
                    EmployeeCode = x.EmployeeCode,
                    ReasonForLeaving = x.ReasonForLeaving,
                    UploadDocumentPath = x.UploadDocument,

                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,

                    CreatedBy = x.CreatedBy ?? 0,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// Asynchronously retrieves the job history details of an employee by their unique identifier.
        /// </summary>
        /// <remarks>This method queries the database for an employee's job history record based on the
        /// provided identifier. If no record is found, the method returns <see langword="null"/>.</remarks>
        /// <param name="id">The unique identifier of the employee job history record to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="EmployeeJobHistoryDto"/> object with the job history details if found; otherwise, <see
        /// langword="null"/>.</returns>
        public async Task<EmployeeJobHistoryDto?> getByIdEmpJobAsync(int id)
        {
            return await _context.EmployeeJobHistories
                .Where(x => x.Id == id)
                .Select(x => new EmployeeJobHistoryDto
                {
                    Id = x.Id,
                    Employer = x.Employer,
                    JobTitle = x.JobTitle,

                    FromDate = x.FromDate.ToDateTime(TimeOnly.MinValue),
                    ToDate = x.ToDate.ToDateTime(TimeOnly.MinValue),

                    LastCTC = x.LastCtc,
                    Website = x.Website,
                    EmployeeCode = x.EmployeeCode,
                    ReasonForLeaving = x.ReasonForLeaving,
                    UploadDocumentPath = x.UploadDocument,

                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,

                    CreatedBy = x.CreatedBy ?? 0,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// Asynchronously adds a new employee job history record to the database.
        /// </summary>
        /// <remarks>This method creates a new <see cref="EmployeeJobHistory"/> entity from the provided
        /// data transfer object and saves it to the database. The <paramref name="model"/> must contain valid data for
        /// all required fields. The method returns the ID of the newly created record upon successful
        /// completion.</remarks>
        /// <param name="model">An <see cref="EmployeeJobHistoryDto"/> object containing the details of the employee's job history to be
        /// added.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the
        /// newly added employee job history record.</returns>
        public async Task<int> addEmpJobAsync(EmployeeJobHistoryDto model)
        {
            var entity = new EmployeeJobHistory
            {
                Employer = model.Employer,
                JobTitle = model.JobTitle,

                // DateTime → DateOnly
                FromDate = DateOnly.FromDateTime(model.FromDate),
                ToDate = DateOnly.FromDateTime(model.ToDate),

                LastCtc = model.LastCTC,
                Website = model.Website,
                EmployeeCode = model.EmployeeCode,
                ReasonForLeaving = model.ReasonForLeaving,

                UploadDocument = model.UploadDocumentPath,

                CompanyId = model.CompanyId,
                RegionId = model.RegionId,
                UserId = model.UserId,

                CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now
            };

            await _context.EmployeeJobHistories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        /// <summary>
        /// Asynchronously updates the job history of an employee with the provided details.
        /// </summary>
        /// <remarks>This method updates the job history record in the database with the details provided
        /// in the <paramref name="model"/>. If the <paramref name="model"/> includes a non-empty document path, the
        /// document is also updated.</remarks>
        /// <param name="model">The <see cref="EmployeeJobHistoryDto"/> containing the updated job history details.</param>
        /// <returns><see langword="true"/> if the employee job history was successfully updated; otherwise, <see
        /// langword="false"/> if the specified job history does not exist.</returns>
        public async Task<bool> updateEmpJobAsync(EmployeeJobHistoryDto model)
        {
            var entity = await _context.EmployeeJobHistories.FindAsync(model.Id);
            if (entity == null) return false;

            entity.Employer = model.Employer;
            entity.JobTitle = model.JobTitle;

            // DateTime → DateOnly
            entity.FromDate = DateOnly.FromDateTime(model.FromDate);
            entity.ToDate = DateOnly.FromDateTime(model.ToDate);

            entity.LastCtc = model.LastCTC;
            entity.Website = model.Website;
            entity.EmployeeCode = model.EmployeeCode;
            entity.ReasonForLeaving = model.ReasonForLeaving;

            if (!string.IsNullOrEmpty(model.UploadDocumentPath))
            {
                entity.UploadDocument = model.UploadDocumentPath;
            }

            entity.CompanyId = model.CompanyId;
            entity.RegionId = model.RegionId;
            entity.UserId = model.UserId;

            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Asynchronously deletes an employee's job history record by its identifier.
        /// </summary>
        /// <remarks>This method removes the specified employee job history record from the database.
        /// Ensure that the identifier corresponds to an existing record.</remarks>
        /// <param name="id">The unique identifier of the employee job history record to delete. Must be a positive integer.</param>
        /// <returns><see langword="true"/> if the record was successfully deleted; otherwise, <see langword="false"/> if no
        /// record with the specified identifier was found.</returns>
        public async Task<bool> deleteEmpJobAsync(int id)
        {
            var entity = await _context.EmployeeJobHistories.FindAsync(id);
            if (entity == null) return false;

            _context.EmployeeJobHistories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion
        #region employee Immigration Details
        public async Task<List<EmployeeImmigrationDto>> GetAllImmigrationAsync()
        {
            return await _context.EmployeeImmigrations
                // .Include(v => v.VisaTypeMaster)
                //.Include(s => s.WorkAuthStatusMaster)
                .Select(x => new EmployeeImmigrationDto
                {
                    ImmigrationId = x.ImmigrationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,

                    EmployeeId = x.EmployeeId,
                    FullName = x.FullName,
                    DateOfBirth = x.DateOfBirth,
                    Nationality = x.Nationality,
                    PassportNumber = x.PassportNumber,
                    PassportExpiryDate = x.PassportExpiryDate,

                    VisaTypeId = x.VisaTypeId,
                      VisaTypeName =x.VisaTypeId!=null? _context.VisaTypeMasters.Where(y=>y.VisaTypeId==x.VisaTypeId).FirstOrDefault().VisaTypeName:null,     // ⭐ Added>x

                    StatusId = x.StatusId,
                    StatusName =x.StatusId!=null? _context.WorkAuthStatusMasters.Where(y=>y.StatusId==x.StatusId).FirstOrDefault().StatusName:null,  // ⭐ Added

                    VisaNumber = x.VisaNumber,
                    VisaIssuingCountry = x.VisaIssuingCountry,
                    VisaIssueDate = x.VisaIssueDate,
                    VisaExpiryDate = x.VisaExpiryDate,
                    EmployerName = x.EmployerName,
                    EmployerAddress = x.EmployerAddress,
                    ContactPerson = x.ContactPerson,
                    EmployerContact = x.EmployerContact,
                    Remarks = x.Remarks
                })
                .ToListAsync();
        }

        public async Task<EmployeeImmigrationDto> GetByIdImmigrationAsync(int id)
        {
            return await _context.EmployeeImmigrations
                //  .Include(v => v.VisaTypeMaster)
                // .Include(s => s.WorkAuthStatusMaster)
                .Where(x => x.ImmigrationId == id)
                .Select(x => new EmployeeImmigrationDto
                {
                    ImmigrationId = x.ImmigrationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,
                    EmployeeId = x.EmployeeId,
                    FullName = x.FullName,
                    DateOfBirth = x.DateOfBirth,
                    Nationality = x.Nationality,
                    PassportNumber = x.PassportNumber,
                    PassportExpiryDate = x.PassportExpiryDate,

                    VisaTypeId = x.VisaTypeId,
                    // VisaTypeName = x.VisaTypeMaster.VisaTypeName,

                    StatusId = x.StatusId,
                    //StatusName = x.WorkAuthStatusMaster.StatusName,

                    VisaNumber = x.VisaNumber,
                    VisaIssuingCountry = x.VisaIssuingCountry,
                    VisaIssueDate = x.VisaIssueDate,
                    VisaExpiryDate = x.VisaExpiryDate,
                    EmployerName = x.EmployerName,
                    EmployerAddress = x.EmployerAddress,
                    ContactPerson = x.ContactPerson,
                    EmployerContact = x.EmployerContact,
                    Remarks = x.Remarks
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> CreateImmigrationAsync(EmployeeImmigrationDto dto)
        {
            var entity = new EmployeeImmigration
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                UserId = dto.UserId,
                EmployeeId = dto.EmployeeId,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                PassportNumber = dto.PassportNumber,
                PassportExpiryDate = dto.PassportExpiryDate,
                VisaTypeId = dto.VisaTypeId,
                StatusId = dto.StatusId,
                VisaNumber = dto.VisaNumber,
                VisaIssuingCountry = dto.VisaIssuingCountry,
                VisaIssueDate = dto.VisaIssueDate,
                VisaExpiryDate = dto.VisaExpiryDate,
                EmployerName = dto.EmployerName,
                EmployerAddress = dto.EmployerAddress,
                ContactPerson = dto.ContactPerson,
                EmployerContact = dto.EmployerContact,
                Remarks = dto.Remarks,
                CreatedDate = DateTime.Now,
                // ⭐ ADD THESE THREE LINES
                PassportCopyPath = dto.PassportCopyPath,
                VisaCopyPath = dto.VisaCopyPath,
                OtherDocumentsPath = dto.OtherDocumentsPath
            };

            _context.EmployeeImmigrations.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateImmigrationAsync(EmployeeImmigrationDto dto)
        {
            var entity = await _context.EmployeeImmigrations.FindAsync(dto.ImmigrationId);

            if (entity == null) return false;

            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.UserId = dto.UserId;
            entity.EmployeeId = dto.EmployeeId;
            entity.FullName = dto.FullName;
            entity.DateOfBirth = dto.DateOfBirth;
            entity.Nationality = dto.Nationality;
            entity.PassportNumber = dto.PassportNumber;
            entity.PassportExpiryDate = dto.PassportExpiryDate;
            entity.VisaTypeId = dto.VisaTypeId;
            entity.StatusId = dto.StatusId;
            entity.VisaNumber = dto.VisaNumber;
            entity.VisaIssuingCountry = dto.VisaIssuingCountry;
            entity.VisaIssueDate = dto.VisaIssueDate;
            entity.VisaExpiryDate = dto.VisaExpiryDate;
            entity.EmployerName = dto.EmployerName;
            entity.EmployerAddress = dto.EmployerAddress;
            entity.ContactPerson = dto.ContactPerson;
            entity.EmployerContact = dto.EmployerContact;
            entity.Remarks = dto.Remarks;
            // ⭐ ADD THESE LINES
            if (dto.PassportCopyPath != null)
                entity.PassportCopyPath = dto.PassportCopyPath;

            if (dto.VisaCopyPath != null)
                entity.VisaCopyPath = dto.VisaCopyPath;

            if (dto.OtherDocumentsPath != null)
                entity.OtherDocumentsPath = dto.OtherDocumentsPath;
            entity.ModifiedDate = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteImmigrationAsync(int id)
        {
            var entity = await _context.EmployeeImmigrations.FindAsync(id);
            if (entity == null) return false;

            _context.EmployeeImmigrations.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<VisaTypeDto>> GetVisaTypesAsync()
        {
            return await _context.VisaTypeMasters
                .Select(v => new VisaTypeDto
                {
                    VisaTypeId = v.VisaTypeId,
                    CompanyId = v.CompanyId,
                    RegionId = v.RegionId,
                    VisaTypeName = v.VisaTypeName
                })
                .ToListAsync();
        }

        public async Task<List<WorkAuthStatusDto>> GetStatusListAsync()
        {
            return await _context.WorkAuthStatusMasters
                .Select(s => new WorkAuthStatusDto
                {
                    StatusId = s.StatusId,
                    CompanyId = s.CompanyId,
                    RegionId = s.RegionId,
                    StatusName = s.StatusName
                })
                .ToListAsync();
        }
        #endregion
        #region employee Document
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeDocumentDto>> getAllempDocAsync()
        {
            return await _context.EmployeeDocuments
                .Select(x => new EmployeeDocumentDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    DocumentNumber = x.DocumentNumber,
                    IssuedDate = x.IssuedDate,
                    ExpiryDate = x.ExpiryDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeDocumentDto>> getByUserIdempDocAsync(int userId)
        {
            return await _context.EmployeeDocuments
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeDocumentDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    DocumentNumber = x.DocumentNumber,
                    IssuedDate = x.IssuedDate,
                    ExpiryDate = x.ExpiryDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmployeeDocumentDto?> getByIdempDocAsync(int id)
        {
            return await _context.EmployeeDocuments
                .Where(x => x.Id == id)
                .Select(x => new EmployeeDocumentDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    DocumentNumber = x.DocumentNumber,
                    IssuedDate = x.IssuedDate,
                    ExpiryDate = x.ExpiryDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> addempDocAsync(EmployeeDocumentDto model)
        {
            var entity = new EmployeeDocument
            {
                RegionId = model.RegionId,
                CompanyId = model.CompanyId,
                UserId = model.UserId,
                DocumentTypeId = model.DocumentTypeId,
                DocumentName = model.DocumentName,
                DocumentNumber = model.DocumentNumber,
                IssuedDate = model.IssuedDate,
                ExpiryDate = model.ExpiryDate,
                FileName = model.FileName,
                FilePath = model.FilePath,
                Remarks = model.Remarks,
                IsConfidential = model.IsConfidential,
                CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now
            };

            await _context.EmployeeDocuments.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> updateempDocAsync(EmployeeDocumentDto model)
        {
            var entity = await _context.EmployeeDocuments.FindAsync(model.Id);
            if (entity == null)
                return false;

            entity.RegionId = model.RegionId;
            entity.CompanyId = model.CompanyId;
            entity.UserId = model.UserId;
            entity.DocumentTypeId = model.DocumentTypeId;
            entity.DocumentName = model.DocumentName;
            entity.DocumentNumber = model.DocumentNumber;
            entity.IssuedDate = model.IssuedDate;
            entity.ExpiryDate = model.ExpiryDate;
            entity.FileName = model.FileName ?? entity.FileName;
            entity.FilePath = model.FilePath ?? entity.FilePath;
            entity.Remarks = model.Remarks;
            entity.IsConfidential = model.IsConfidential;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> deleteempDocAsync(int id)
        {
            var entity = await _context.EmployeeDocuments.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return false;

            _context.EmployeeDocuments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DocumentTypeDto>> GetActiveDocumentTypesAsync()
        {
            return await _context.DocumentTypes
                .Where(d => d.IsActive == true)
                .Select(d => new DocumentTypeDto
                {
                    Id = d.Id,
                    TypeName = d.TypeName,
                    IsActive = d.IsActive
                })
                .ToListAsync();
        }
        #endregion
        #region employee Form Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeFormDto>> getAllempFormAsync()
        {
            return await _context.EmployeeForms
                .Select(x => new EmployeeFormDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    EmployeeCode = x.EmployeeCode,
                    IssueDate = x.IssueDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeFormDto>> getByUserIdempFormAsync(int userId)
        {
            return await _context.EmployeeForms
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeFormDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    EmployeeCode = x.EmployeeCode,
                    IssueDate = x.IssueDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmployeeFormDto?> getByIdempFormAsync(int id)
        {
            return await _context.EmployeeForms
                .Where(x => x.Id == id)
                .Select(x => new EmployeeFormDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    EmployeeCode = x.EmployeeCode,
                    IssueDate = x.IssueDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> addempFormAsync(EmployeeFormDto model)
        {
            var entity = new EmployeeForm
            {
                RegionId = model.RegionId,
                CompanyId = model.CompanyId,
                UserId = model.UserId,
                DocumentTypeId = model.DocumentTypeId,
                DocumentName = model.DocumentName,
                EmployeeCode = model.EmployeeCode,
                IssueDate = model.IssueDate,
                FileName = model.FileName,
                FilePath = model.FilePath,
                Remarks = model.Remarks,
                IsConfidential = model.IsConfidential,
                CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now
            };

            await _context.EmployeeForms.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> updateempFormAsync(EmployeeFormDto model)
        {
            var entity = await _context.EmployeeForms.FindAsync(model.Id);
            if (entity == null)
                return false;

            entity.RegionId = model.RegionId;
            entity.CompanyId = model.CompanyId;
            entity.UserId = model.UserId;
            entity.DocumentTypeId = model.DocumentTypeId;
            entity.DocumentName = model.DocumentName;
            entity.EmployeeCode = model.EmployeeCode;
            entity.IssueDate = model.IssueDate;
            entity.FileName = model.FileName;
            entity.FilePath = model.FilePath ?? entity.FilePath;
            entity.Remarks = model.Remarks;
            entity.IsConfidential = model.IsConfidential;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> deleteempFormAsync(int id)
        {
            var entity = await _context.EmployeeForms.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return false;

            _context.EmployeeForms.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
        #region employee Letter details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeLetterDto>> getAllempLetterAsync()
        {
            return await _context.EmployeeLetters
                .Select(x => new EmployeeLetterDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    EmployeeCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,
                    IssuedDate = x.IssuedDate,
                    ValidityDate = x.ValidityDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeLetterDto>> getByUserIdempLetterAsync(int userId)
        {
            return await _context.EmployeeLetters
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeLetterDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    EmployeeCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,
                    IssuedDate = x.IssuedDate,
                    ValidityDate = x.ValidityDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<EmployeeLetterDto?> getByIdempLetterAsync(int id)
        {
            return await _context.EmployeeLetters
                .Where(x => x.Id == id)
                .Select(x => new EmployeeLetterDto
                {
                    Id = x.Id,
                    RegionId = x.RegionId,
                    CompanyId = x.CompanyId,
                    UserId = x.UserId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentName = x.DocumentName,
                    EmployeeCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,
                    IssuedDate = x.IssuedDate,
                    ValidityDate = x.ValidityDate,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Remarks = x.Remarks,
                    IsConfidential = x.IsConfidential,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> addempLetterAsync(EmployeeLetterDto model)
        {
            var entity = new EmployeeLetter
            {
                RegionId = model.RegionId,
                CompanyId = model.CompanyId,
                UserId = model.UserId,
                DocumentTypeId = model.DocumentTypeId,
                DocumentName = model.DocumentName,
                EmployeeCode = model.EmployeeCode,
                EmployeeName = model.EmployeeName,
                IssuedDate = model.IssuedDate,
                ValidityDate = model.ValidityDate,
                FileName = model.FileName,
                FilePath = model.FilePath,
                Remarks = model.Remarks,
                IsConfidential = model.IsConfidential,
                CreatedBy = model.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _context.EmployeeLetters.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> updateempLetterAsync(EmployeeLetterDto model)
        {
            var entity = await _context.EmployeeLetters.FindAsync(model.Id);
            if (entity == null)
                return false;

            entity.RegionId = model.RegionId;
            entity.CompanyId = model.CompanyId;
            entity.UserId = model.UserId;
            entity.DocumentTypeId = model.DocumentTypeId;
            entity.DocumentName = model.DocumentName;
            entity.EmployeeCode = model.EmployeeCode;
            entity.EmployeeName = model.EmployeeName;
            entity.IssuedDate = model.IssuedDate;
            entity.ValidityDate = model.ValidityDate;
            entity.FileName = model.FileName;
            entity.FilePath = model.FilePath ?? entity.FilePath;
            entity.Remarks = model.Remarks;
            entity.IsConfidential = model.IsConfidential;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteempLetterAsync(int id)
        {
            var entity = await _context.EmployeeLetters.FindAsync(id);
            if (entity == null)
                return false;

            _context.EmployeeLetters.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }





        #endregion
        #region employee Bank Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeBankDetailsDto>> getAllempBankAsync()
        {
            var data = await _unitOfWork.Repository<EmployeeBankDetail>().GetAllAsync();

            return data.Select(e => new EmployeeBankDetailsDto
            {
                BankDetailsId = e.BankDetailsId,
                EmployeeId = e.EmployeeId,
                RegionId = e.RegionId,
                UserId = e.UserId,
                CompanyId = e.CompanyId,
                BankName = e.BankName,
                BranchName = e.BranchName,
                AccountHolderName = e.AccountHolderName,
                AccountNumber = e.AccountNumber,
                AccountTypeId = Convert.ToInt32(e.AccountTypeId),
                Ifsccode = e.Ifsccode,
                Micrcode = e.Micrcode,
                Upiid = e.Upiid
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<EmployeeBankDetailsDto?> getByIdempBankAsync(int id)
        {
            var e = await _unitOfWork.Repository<EmployeeBankDetail>().GetByIdAsync(id);
            if (e is null) return null;

            return new EmployeeBankDetailsDto
            {
                BankDetailsId = e.BankDetailsId,
                EmployeeId = e.EmployeeId,
                RegionId = e.RegionId,
                UserId = e.UserId,
                CompanyId = e.CompanyId,
                BankName = e.BankName,
                BranchName = e.BranchName,
                AccountHolderName = e.AccountHolderName,
                AccountNumber = e.AccountNumber,
                AccountTypeId = Convert.ToInt32(e.AccountTypeId),
                Ifsccode = e.Ifsccode,
                Micrcode = e.Micrcode,
                Upiid = e.Upiid
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public async Task<bool> addempBankAsync(EmployeeBankDetailsDto dto)
        {
            try
            {
                var entity = new EmployeeBankDetail
                {
                    EmployeeId = dto.EmployeeId,
                    RegionId = dto.RegionId,
                    UserId = dto.UserId,
                    CompanyId = dto.CompanyId,
                    BankName = dto.BankName,
                    BranchName = dto.BranchName,
                    AccountHolderName = dto.AccountHolderName,
                    AccountNumber = dto.AccountNumber,
                    AccountTypeId = dto.AccountTypeId,
                    Ifsccode = dto.Ifsccode,
                    Micrcode = dto.Micrcode,
                    Upiid = dto.Upiid,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<EmployeeBankDetail>().AddAsync(entity);
                return await _unitOfWork.CompleteAsync() > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> updateempBankAsync(EmployeeBankDetailsDto dto)
        {
            var repo = _unitOfWork.Repository<EmployeeBankDetail>();
            var entity = await repo.GetByIdAsync(dto.BankDetailsId);

            if (entity is null) return false;

            entity.RegionId = dto.RegionId;
            entity.UserId = dto.UserId;
            entity.CompanyId = dto.CompanyId;
            entity.BankName = dto.BankName;
            entity.BranchName = dto.BranchName;
            entity.AccountHolderName = dto.AccountHolderName;
            entity.AccountNumber = dto.AccountNumber;
            entity.AccountTypeId = dto.AccountTypeId; entity.Ifsccode = dto.Ifsccode;
            entity.Micrcode = dto.Micrcode;
            entity.Upiid = dto.Upiid;
            entity.ModifiedAt = DateTime.UtcNow;

            repo.Update(entity);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<bool> deleteempBankAsync(int id)
        {
            var repo = _unitOfWork.Repository<EmployeeBankDetail>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null) return false;

            repo.Remove(entity);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        #endregion
        #region employee DD List
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeDdlistDto>> getAllempDDAsync()
        {
            var data = await _context.EmployeeDdlists.ToListAsync();

            return data.Select(e => new EmployeeDdlistDto
            {
                DdlistId = e.DdlistId,
                EmployeeId = e.EmployeeId,
                RegionId = e.RegionId,
                UserId = e.UserId,
                CompanyId = e.CompanyId,
                Ddnumber = e.Ddnumber,
                Dddate = e.Dddate,
                BankName = e.BankName,
                BranchName = e.BranchName,
                Amount = e.Amount,
                PayeeName = e.PayeeName,
                DdcopyFilePath = e.DdcopyFilePath
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmployeeDdlistDto?> getByIdempDDAsync(int id)
        {
            var e = await _context.EmployeeDdlists.FindAsync(id);
            if (e == null) return null;

            return new EmployeeDdlistDto
            {
                DdlistId = e.DdlistId,
                EmployeeId = e.EmployeeId,
                RegionId = e.RegionId,
                UserId = e.UserId,
                CompanyId = e.CompanyId,
                Ddnumber = e.Ddnumber,
                Dddate = e.Dddate,
                BankName = e.BankName,
                BranchName = e.BranchName,
                Amount = e.Amount,
                PayeeName = e.PayeeName,
                DdcopyFilePath = e.DdcopyFilePath
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public async Task<bool> addempDDAsync(EmployeeDdlistDto dto)
        {
            var entity = new EmployeeDdlist
            {
                EmployeeId = dto.EmployeeId,
                RegionId = dto.RegionId,
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                Ddnumber = dto.Ddnumber,
                Dddate = dto.Dddate,
                BankName = dto.BankName,
                BranchName = dto.BranchName,
                Amount = dto.Amount,
                PayeeName = dto.PayeeName,
                DdcopyFilePath = dto.DdcopyFilePath,
                CreatedAt = DateTime.UtcNow
            };

            _context.EmployeeDdlists.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public async Task<bool> updateempDDAsync(EmployeeDdlistDto dto)
        {
            var entity = await _context.EmployeeDdlists.FindAsync(dto.DdlistId);
            if (entity == null) return false;

            entity.EmployeeId = dto.EmployeeId;
            entity.RegionId = dto.RegionId;
            entity.UserId = dto.UserId;
            entity.CompanyId = dto.CompanyId;
            entity.Ddnumber = dto.Ddnumber;
            entity.Dddate = dto.Dddate;
            entity.BankName = dto.BankName;
            entity.BranchName = dto.BranchName;
            entity.Amount = dto.Amount;
            entity.PayeeName = dto.PayeeName;
            entity.DdcopyFilePath = dto.DdcopyFilePath;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.EmployeeDdlists.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> deleteempDDAsync(int id)
        {
            var entity = await _context.EmployeeDdlists.FindAsync(id);
            if (entity == null) return false;

            _context.EmployeeDdlists.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
        #region employee w4 details
        public async Task<List<EmployeeW4Dto>> getAllempW4Async()
        {
            return await _context.EmployeeW4s
                .Select(w => new EmployeeW4Dto
                {
                    W4Id = w.W4Id,
                    EmployeeId = w.EmployeeId,
                    FirstName = w.FirstName,
                    MiddleInitial = w.MiddleInitial,
                    LastName = w.LastName,
                    Ssn = w.Ssn,
                    Address = w.Address,
                    City = w.City,
                    State = w.State,
                    ZipCode = w.ZipCode,
                    FilingStatus = w.FilingStatus,
                    MultipleJobsOrSpouse = w.MultipleJobsOrSpouse,
                    TotalDependents = w.TotalDependents,
                    DependentAmounts = w.DependentAmounts,
                    OtherIncome = w.OtherIncome,
                    Deductions = w.Deductions,
                    ExtraWithholding = w.ExtraWithholding,
                    EmployeeSignature = w.EmployeeSignature,
                    FormDate = w.FormDate,
                    RegionId = w.RegionId,
                    UserId = w.UserId,
                    CompanyId = w.CompanyId
                })
                .ToListAsync();
        }

        public async Task<EmployeeW4Dto?> getByIdempW4Async(int id)
        {
            var w4 = await _context.EmployeeW4s.FindAsync(id);
            if (w4 == null) return null;
            return new EmployeeW4Dto
            {
                W4Id = w4.W4Id,
                EmployeeId = w4.EmployeeId,
                FirstName = w4.FirstName,
                MiddleInitial = w4.MiddleInitial,
                LastName = w4.LastName,
                Ssn = w4.Ssn,
                Address = w4.Address,
                City = w4.City,
                State = w4.State,
                ZipCode = w4.ZipCode,
                FilingStatus = w4.FilingStatus,
                MultipleJobsOrSpouse = w4.MultipleJobsOrSpouse,
                TotalDependents = w4.TotalDependents,
                DependentAmounts = w4.DependentAmounts,
                OtherIncome = w4.OtherIncome,
                Deductions = w4.Deductions,
                ExtraWithholding = w4.ExtraWithholding,
                EmployeeSignature = w4.EmployeeSignature,
                FormDate = w4.FormDate,
                RegionId = w4.RegionId,
                UserId = w4.UserId,
                CompanyId = w4.CompanyId
            };
        }

        public async Task<bool> addempW4Async(EmployeeW4Dto dto)
        {
            var entity = new EmployeeW4
            {
                EmployeeId = dto.EmployeeId,
                FirstName = dto.FirstName,
                MiddleInitial = dto.MiddleInitial,
                LastName = dto.LastName,
                Ssn = dto.Ssn,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                FilingStatus = dto.FilingStatus,
                MultipleJobsOrSpouse = dto.MultipleJobsOrSpouse,
                TotalDependents = dto.TotalDependents,
                DependentAmounts = dto.DependentAmounts,
                OtherIncome = dto.OtherIncome,
                Deductions = dto.Deductions,
                ExtraWithholding = dto.ExtraWithholding,
                EmployeeSignature = dto.EmployeeSignature,
                FormDate = dto.FormDate,
                RegionId = dto.RegionId,
                UserId = dto.UserId,
                CompanyId = dto.CompanyId
            };
            _context.EmployeeW4s.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> updateempW4Async(EmployeeW4Dto dto)
        {
            var entity = await _context.EmployeeW4s.FindAsync(dto.W4Id);
            if (entity == null) return false;

            entity.EmployeeId = dto.EmployeeId;
            entity.FirstName = dto.FirstName;
            entity.MiddleInitial = dto.MiddleInitial;
            entity.LastName = dto.LastName;
            entity.Ssn = dto.Ssn;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.State = dto.State;
            entity.ZipCode = dto.ZipCode;
            entity.FilingStatus = dto.FilingStatus;
            entity.MultipleJobsOrSpouse = dto.MultipleJobsOrSpouse;
            entity.TotalDependents = dto.TotalDependents;
            entity.DependentAmounts = dto.DependentAmounts;
            entity.OtherIncome = dto.OtherIncome;
            entity.Deductions = dto.Deductions;
            entity.ExtraWithholding = dto.ExtraWithholding;
            entity.EmployeeSignature = dto.EmployeeSignature;
            entity.FormDate = dto.FormDate;
            entity.RegionId = dto.RegionId;
            entity.UserId = dto.UserId;
            entity.CompanyId = dto.CompanyId;

            _context.EmployeeW4s.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> deleteempW4Async(int id)
        {
            var entity = await _context.EmployeeW4s.FindAsync(id);
            if (entity == null) return false;
            _context.EmployeeW4s.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
        #region employee Emergency Details
        public async Task<IEnumerable<EmployeeEmergencyContactDto>> GetAllempEmerAsync()
        {
            var query = from e in _context.EmployeeEmergencyContacts
                        join r in _context.Relationships on e.RelationshipId equals r.RelationshipId into gj
                        from rel in gj.DefaultIfEmpty()
                        select new EmployeeEmergencyContactDto
                        {
                            EmergencyContactId = e.EmergencyContactId,

                            UserId = e.UserId,
                            CompanyId = e.CompanyId,
                            RegionId = e.RegionId,
                            ContactName = e.ContactName,
                            RelationshipId = e.RelationshipId,
                            Relationship = rel != null ? rel.RelationshipName : null,
                            PhoneNumber = e.PhoneNumber,
                            AlternatePhone = e.AlternatePhone,
                            Email = e.Email,
                            Address = e.Address,
                            CreatedBy = e.CreatedBy,
                            CreatedDate = e.CreatedDate,
                            ModifiedBy = e.ModifiedBy,
                            ModifiedDate = e.ModifiedDate
                        };

            return await query.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeEmergencyContactDto>> GetByUserIdempEmerAsync(int userId)
        {
            var query = from e in _context.EmployeeEmergencyContacts
                        where e.UserId == userId
                        join r in _context.Relationships on e.RelationshipId equals r.RelationshipId into gj
                        from rel in gj.DefaultIfEmpty()
                        select new EmployeeEmergencyContactDto
                        {
                            EmergencyContactId = e.EmergencyContactId,

                            UserId = e.UserId,
                            CompanyId = e.CompanyId,
                            RegionId = e.RegionId,
                            ContactName = e.ContactName,
                            RelationshipId = e.RelationshipId,
                            Relationship = rel != null ? rel.RelationshipName : null,
                            PhoneNumber = e.PhoneNumber,
                            AlternatePhone = e.AlternatePhone,
                            Email = e.Email,
                            Address = e.Address,
                            CreatedBy = e.CreatedBy,
                            CreatedDate = e.CreatedDate,
                            ModifiedBy = e.ModifiedBy,
                            ModifiedDate = e.ModifiedDate
                        };

            return await query.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<EmployeeEmergencyContactDto?> GetByIdempEmerAsync(int emergencyContactId)
        {
            var query = from e in _context.EmployeeEmergencyContacts
                        where e.EmergencyContactId == emergencyContactId
                        join r in _context.Relationships on e.RelationshipId equals r.RelationshipId into gj
                        from rel in gj.DefaultIfEmpty()
                        select new EmployeeEmergencyContactDto
                        {
                            EmergencyContactId = e.EmergencyContactId,

                            UserId = e.UserId,
                            CompanyId = e.CompanyId,
                            RegionId = e.RegionId,
                            ContactName = e.ContactName,
                            RelationshipId = e.RelationshipId,
                            Relationship = rel != null ? rel.RelationshipName : null,
                            PhoneNumber = e.PhoneNumber,
                            AlternatePhone = e.AlternatePhone,
                            Email = e.Email,
                            Address = e.Address,
                            CreatedBy = e.CreatedBy,
                            CreatedDate = e.CreatedDate,
                            ModifiedBy = e.ModifiedBy,
                            ModifiedDate = e.ModifiedDate
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> AddempEmerAsync(EmployeeEmergencyContactDto model)
        {
            try
            {
                // Prefer relationship name from master if ID provided
                string relationshipName = model.Relationship ?? string.Empty;
                if (model.RelationshipId > 0)
                {
                    var relFromMaster = await _context.Relationships
                        .Where(r => r.RelationshipId == model.RelationshipId)
                        .Select(r => r.RelationshipName)
                        .FirstOrDefaultAsync();

                    if (!string.IsNullOrEmpty(relFromMaster))
                        relationshipName = relFromMaster;
                }

                var entity = new EmployeeEmergencyContact
                {

                    UserId = model.UserId,
                    CompanyId = model.CompanyId,
                    RegionId = model.RegionId,
                    ContactName = model.ContactName ?? string.Empty,
                    RelationshipId = model.RelationshipId,
                    // if your EF model has a Relationship string column, set it; else it's optional
                    // assuming EF model only has RelationshipId (but you had navigation property)
                    PhoneNumber = model.PhoneNumber,
                    AlternatePhone = model.AlternatePhone,
                    Email = model.Email,
                    Address = model.Address,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = model.CreatedDate ?? DateTime.Now
                };

                await _context.EmployeeEmergencyContacts.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity.EmergencyContactId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateempEmerAsync(EmployeeEmergencyContactDto model)
        {
            var entity = await _context.EmployeeEmergencyContacts.FindAsync(model.EmergencyContactId);
            if (entity == null)
                return false;


            entity.UserId = model.UserId;
            entity.CompanyId = model.CompanyId;
            entity.RegionId = model.RegionId;
            entity.ContactName = model.ContactName ?? entity.ContactName;
            entity.RelationshipId = model.RelationshipId != 0 ? model.RelationshipId : entity.RelationshipId;

            // try update relationship name if needed (same pattern as family)
            if (model.RelationshipId > 0)
            {
                var relFromMaster = await _context.Relationships
                    .Where(r => r.RelationshipId == model.RelationshipId)
                    .Select(r => r.RelationshipName)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(relFromMaster))
                {
                    // If you have a Relationship string column on the EF model, update it.
                    // entity.Relationship = relFromMaster; // uncomment if present
                }
            }
            else if (!string.IsNullOrEmpty(model.Relationship))
            {
                // entity.Relationship = model.Relationship; // uncomment if present
            }

            entity.PhoneNumber = model.PhoneNumber ?? entity.PhoneNumber;
            entity.AlternatePhone = model.AlternatePhone ?? entity.AlternatePhone;
            entity.Email = model.Email ?? entity.Email;
            entity.Address = model.Address ?? entity.Address;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteempEmerAsync(int emergencyContactId)
        {
            var entity = await _context.EmployeeEmergencyContacts.FirstOrDefaultAsync(x => x.EmergencyContactId == emergencyContactId);
            if (entity == null)
                return false;

            _context.EmployeeEmergencyContacts.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RelationshipDto>> GetRelationshipListAsync()
        {
            return await _context.Relationships
                .Where(r => r.IsActive && !r.IsDeleted)
                .Select(r => new RelationshipDto
                {
                    RelationshipID = r.RelationshipId,
                    RelationshipName = r.RelationshipName
                })
                .OrderBy(r => r.RelationshipName)
                .ToListAsync();
        }
        #endregion
        #region employee Family details
        // NOTE: EmployeeFamilyDetail.DateOfBirth is DateOnly in your EF model,
        // while DTO uses DateTime. Convert explicitly.

        public async Task<IEnumerable<EmployeeFamilyDto>> getAllempFamilyAsync()
        {
            var query = from f in _context.EmployeeFamilyDetails
                        join r in _context.Relationships
                            on f.RelationshipId equals r.RelationshipId into gj
                        from rel in gj.DefaultIfEmpty()
                        select new EmployeeFamilyDto
                        {
                            FamilyId = f.FamilyId,
                            UserId = f.UserId,
                            CompanyId = f.CompanyId,       // use CompanyId (camel-case) to match EF model
                            RegionId = f.RegionId,         // use RegionId
                            Name = f.Name,
                            RelationshipId = f.RelationshipId,
                            Relationship = rel != null ? rel.RelationshipName : null, // left-join rel name
                            // Convert DateOnly -> DateTime
                            DateOfBirth = f.DateOfBirth.ToDateTime(System.TimeOnly.MinValue),
                            Gender = f.Gender,
                            GenderId = f.GenderId,
                            Occupation = f.Occupation,
                            Phone = f.Phone,
                            Address = f.Address,
                            IsDependent = f.IsDependent,
                            CreatedBy = f.CreatedBy,
                            CreatedDate = f.CreatedDate,
                            ModifiedBy = f.ModifiedBy,
                            ModifiedDate = f.ModifiedDate
                        };

            return await query.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeFamilyDto>> getByUserIdempFamilyAsync(int userId)
        {
            var query = from f in _context.EmployeeFamilyDetails
                        where f.UserId == userId
                        join r in _context.Relationships
                            on f.RelationshipId equals r.RelationshipId into gj
                        from rel in gj.DefaultIfEmpty()
                        select new EmployeeFamilyDto
                        {
                            FamilyId = f.FamilyId,
                            UserId = f.UserId,
                            CompanyId = f.CompanyId,
                            RegionId = f.RegionId,
                            Name = f.Name,
                            RelationshipId = f.RelationshipId,
                            Relationship = f != null ? f.Relationship : null,
                            DateOfBirth = f.DateOfBirth.ToDateTime(System.TimeOnly.MinValue),
                            Gender = f.Gender,
                            GenderId = f.GenderId,
                            Occupation = f.Occupation,
                            Phone = f.Phone,
                            Address = f.Address,
                            IsDependent = f.IsDependent,
                            CreatedBy = f.CreatedBy,
                            CreatedDate = f.CreatedDate,
                            ModifiedBy = f.ModifiedBy,
                            ModifiedDate = f.ModifiedDate
                        };

            return await query.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public async Task<EmployeeFamilyDto?> getByIdempFamilyAsync(int familyId)
        {
            var query = from f in _context.EmployeeFamilyDetails
                        where f.FamilyId == familyId
                        join r in _context.Relationships
                            on f.RelationshipId equals r.RelationshipId into gj
                        from rel in gj.DefaultIfEmpty()
                        select new EmployeeFamilyDto
                        {
                            FamilyId = f.FamilyId,
                            UserId = f.UserId,
                            CompanyId = f.CompanyId,
                            RegionId = f.RegionId,
                            Name = f.Name,
                            RelationshipId = f.RelationshipId,
                            Relationship = rel != null ? rel.RelationshipName : null,
                            DateOfBirth = f.DateOfBirth.ToDateTime(System.TimeOnly.MinValue),
                            Gender = f.Gender,
                            GenderId = f.GenderId,
                            Occupation = f.Occupation,
                            Phone = f.Phone,
                            Address = f.Address,
                            IsDependent = f.IsDependent,
                            CreatedBy = f.CreatedBy,
                            CreatedDate = f.CreatedDate,
                            ModifiedBy = f.ModifiedBy,
                            ModifiedDate = f.ModifiedDate
                        };

            return await query.FirstOrDefaultAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> addempFamilyAsync(EmployeeFamilyDto model)
        {
            // Start with provided relationship string if any
            string relationshipName = model.Relationship ?? string.Empty;

            // If caller provided a RelationshipId, try to get the master value
            if (model.RelationshipId.HasValue)
            {
                var relFromMaster = await _context.Relationships
                    .Where(r => r.RelationshipId == model.RelationshipId.Value)
                    .Select(r => r.RelationshipName)
                    .FirstOrDefaultAsync();

                // If we found a name in master, use it; otherwise keep whatever model.Relationship had
                if (!string.IsNullOrEmpty(relFromMaster))
                    relationshipName = relFromMaster;
            }

            // Ensure non-null (DB column is NOT NULL)
            if (relationshipName == null)
                relationshipName = string.Empty;

            var entity = new EmployeeFamilyDetail
            {
                UserId = model.UserId,
                CompanyId = model.CompanyId,
                RegionId = model.RegionId,
                Name = model.Name,
                Relationship = relationshipName,            // <-- guaranteed non-null
                RelationshipId = model.RelationshipId,
                DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth),
                Gender = model.Gender,
                GenderId = model.GenderId,
                Occupation = model.Occupation,
                Phone = model.Phone,
                Address = model.Address,
                IsDependent = model.IsDependent,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };

            await _context.EmployeeFamilyDetails.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.FamilyId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> updateempFamilyAsync(EmployeeFamilyDto model)
        {
            var entity = await _context.EmployeeFamilyDetails.FindAsync(model.FamilyId);
            if (entity == null)
                return false;

            entity.UserId = model.UserId;
            entity.CompanyId = model.CompanyId;
            entity.RegionId = model.RegionId;
            entity.Name = model.Name;

            // Update Relationship name if RelationshipId provided (preferred)
            if (model.RelationshipId.HasValue)
            {
                var relFromMaster = await _context.Relationships
                    .Where(r => r.RelationshipId == model.RelationshipId.Value)
                    .Select(r => r.RelationshipName)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(relFromMaster))
                    entity.Relationship = relFromMaster;
                else
                    entity.Relationship = model.Relationship ?? entity.Relationship ?? string.Empty;
            }
            else if (!string.IsNullOrEmpty(model.Relationship))
            {
                entity.Relationship = model.Relationship;
            }
            // else keep existing entity.Relationship

            if (model.DateOfBirth != default)
                entity.DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth);

            entity.Gender = model.Gender ?? entity.Gender;
            entity.GenderId = model.GenderId ?? entity.GenderId;
            entity.Occupation = model.Occupation ?? entity.Occupation;
            entity.Phone = model.Phone ?? entity.Phone;
            entity.Address = model.Address ?? entity.Address;
            entity.IsDependent = model.IsDependent;
            entity.ModifiedBy = model.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        public async Task<bool> deleteempFamilyAsync(int familyId)
        {
            var entity = await _context.EmployeeFamilyDetails.FirstOrDefaultAsync(x => x.FamilyId == familyId);
            if (entity == null)
                return false;

            _context.EmployeeFamilyDetails.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<IEnumerable<RelationshipDto>> GetRelationshipListAsync()
        //{
        //    // Use the Relationships DbSet to get active relationships
        //    return await _context.Relationships
        //        .Where(r => r.IsActive && !r.IsDeleted)
        //        .Select(r => new RelationshipDto
        //        {
        //            RelationshipID = r.RelationshipId,
        //            RelationshipName = r.RelationshipName
        //        })
        //        .OrderBy(r => r.RelationshipName)
        //        .ToListAsync();
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DropdownGenderDto>> GetGenderListAsync()
        {
            return await _context.Genders
                .Where(g => g.IsActive && !g.IsDeleted)
                .Select(g => new DropdownGenderDto
                {
                    GenderId = g.GenderId,
                    GenderName = g.GenderName
                })
                .OrderBy(g => g.GenderName)
                .ToListAsync();
        }
        #endregion
        #region employee Reference Details
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeReferenceDto>> getAllempRefAsync()
        {
            var q = _context.EmployeeReferences
                .Select(r => new EmployeeReferenceDto
                {
                    ReferenceId = r.ReferenceId,
                    RegionId = r.RegionId,
                    CompanyId = r.CompanyId,
                    Name = r.Name,
                    Title = r.TitleOrDesignation,
                    CompanyName = r.CompanyName,
                    Email = r.EmailId,
                    MobileNumber = r.MobileNumber,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    UserId = r.UserId
                });

            return await q.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeReferenceDto>> getByUserIdempRefAsync(int userId)
        {
            var q = _context.EmployeeReferences
                .Where(r => r.UserId == userId)
                .Select(r => new EmployeeReferenceDto
                {
                    ReferenceId = r.ReferenceId,
                    RegionId = r.RegionId,
                    CompanyId = r.CompanyId,
                    Name = r.Name,
                    Title = r.TitleOrDesignation,
                    CompanyName = r.CompanyName,
                    Email = r.EmailId,
                    MobileNumber = r.MobileNumber,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    UserId = r.UserId
                });

            return await q.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public async Task<EmployeeReferenceDto?> getByIdempRefAsync(int referenceId)
        {
            var r = await _context.EmployeeReferences
                .Where(x => x.ReferenceId == referenceId)
                .Select(r => new EmployeeReferenceDto
                {
                    ReferenceId = r.ReferenceId,
                    RegionId = r.RegionId,
                    CompanyId = r.CompanyId,
                    Name = r.Name,
                    Title = r.TitleOrDesignation,
                    CompanyName = r.CompanyName,
                    Email = r.EmailId,
                    MobileNumber = r.MobileNumber,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    UserId = r.UserId
                })
                .FirstOrDefaultAsync();

            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> addempRefAsync(EmployeeReferenceDto model)
        {
            // map DTO -> EF entity
            var entity = new EmployeeReference
            {
                RegionId = model.RegionId,
                CompanyId = model.CompanyId,
                Name = model.Name,
                TitleOrDesignation = model.Title,
                CompanyName = model.CompanyName,
                EmailId = model.Email,
                MobileNumber = model.MobileNumber,
                CreatedAt = DateTime.Now,
                CreatedBy = model.CreatedBy,
                UserId = model.UserId
            };

            await _context.EmployeeReferences.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ReferenceId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> updateempRefAsync(EmployeeReferenceDto model)
        {
            var entity = await _context.EmployeeReferences.FindAsync(model.ReferenceId);
            if (entity == null)
                return false;

            entity.RegionId = model.RegionId;
            entity.CompanyId = model.CompanyId;
            entity.Name = model.Name;
            entity.TitleOrDesignation = model.Title;
            entity.CompanyName = model.CompanyName;
            entity.EmailId = model.Email;
            entity.MobileNumber = model.MobileNumber;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = model.ModifiedBy;
            // userId typically shouldn't change; but update if provided
            entity.UserId = model.UserId;

            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        public async Task<bool> deleteempRefAsync(int referenceId)
        {
            var entity = await _context.EmployeeReferences.FirstOrDefaultAsync(x => x.ReferenceId == referenceId);
            if (entity == null) return false;

            _context.EmployeeReferences.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region employee Personal Details
        public async Task<IEnumerable<PersonalDetailsDto>> GetAllPersonalEmailAsync()
        {
            var data = await _unitOfWork.Repository<EmployeePersonalDetail>().GetAllAsync();
            return data.Select(MapToDto).ToList();
        }

        public async Task<PersonalDetailsDto?> GetByIdPersonalEmailAsync(int id)
        {
            var entity = await _unitOfWork.Repository<EmployeePersonalDetail>().GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<IEnumerable<PersonalDetailsDto>> SearchPersonalEmailAsync(object filter)
        {
            var props = filter.GetType().GetProperties();
            var all = (await _unitOfWork.Repository<EmployeePersonalDetail>().GetAllAsync()).AsQueryable();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(filter);

                if (value != null)
                {
                    switch (name)
                    {
                        case nameof(EmployeePersonalDetail.FirstName):
                            all = all.Where(x => x.FirstName.Contains(value.ToString()!));
                            break;
                        case nameof(EmployeePersonalDetail.LastName):
                            all = all.Where(x => x.LastName.Contains(value.ToString()!));
                            break;
                        case nameof(EmployeePersonalDetail.MobileNumber):
                            all = all.Where(x => x.MobileNumber.Contains(value.ToString()!));
                            break;
                        case nameof(EmployeePersonalDetail.PersonalEmail):
                            all = all.Where(x => x.PersonalEmail.Contains(value.ToString()!));
                            break;
                        case nameof(EmployeePersonalDetail.CompanyId):
                            all = all.Where(x => x.CompanyId == (int)value);
                            break;
                        case nameof(EmployeePersonalDetail.RegionId):
                            all = all.Where(x => x.RegionId == (int)value);
                            break;

                    }
                }
            }

            return all.Select(MapToDto).ToList();
        }
        // GET by user id
        public async Task<PersonalDetailsDto?> GetByUserIdempProfileAsync(int userId)
        {
            // Assuming repository doesn't have a GetBy predicate, we fetch all and filter.
            // If your repository supports Query or Find, use that for efficiency.
            var all = await _unitOfWork.Repository<EmployeePersonalDetail>().GetAllAsync();
            var entity = all.FirstOrDefault(x => x.UserId == userId);

            return entity == null ? null : MapToDto(entity);
        }
        public async Task<PersonalDetailsDto> AddPersonalEmailAsync(PersonalDetailsDto dto)
        {
            var entity = new EmployeePersonalDetail
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                GenderId = dto.genderId,
                MobileNumber = dto.MobileNumber,
                PersonalEmail = dto.PersonalEmail,
                PermanentAddress = dto.PermanentAddress,
                PresentAddress = dto.PresentAddress,
                Pannumber = dto.PanNumber,
                AadhaarNumber = dto.AadhaarNumber,
                PassportNumber = dto.PassportNumber,
                PlaceOfBirth = dto.PlaceOfBirth,
                Uan = dto.Uan,
                BloodGroup = dto.BloodGroup,
                Citizenship = dto.Citizenship,
                Religion = dto.Religion,
                DrivingLicence = dto.DrivingLicence,
                MaritalStatusId = dto.maritalStatusId,
                MarriageDate = dto.MarriageDate,
                WorkPhone = dto.WorkPhone,
                LinkedInProfile = dto.LinkedInProfile,
                PreviousExperienceText = dto.PreviousExperience,
                ProfilePictureName = dto.profilePicturePath,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                BandGrade = dto.brandGrade,
                EsicNumber = dto.esicNumber,
                Pfnumber = dto.pfNumber,
                EmployeeType = dto.employmentType,
                DateOfJoining = dto.dateofJoining,
                UserId = dto.userId,
                CreatedBy = dto.userId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<EmployeePersonalDetail>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        public async Task<PersonalDetailsDto> UpdateempPersonalAsync(PersonalDetailsDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Repository<EmployeePersonalDetail>()
                    .GetByIdAsync(dto.Id);

                if (entity == null) throw new Exception("Personal details not found");

                entity.FirstName = dto.FirstName;
                entity.LastName = dto.LastName;
                entity.DateOfBirth = dto.DateOfBirth;
                entity.GenderId = dto.genderId;
                entity.MobileNumber = dto.MobileNumber;
                entity.PersonalEmail = dto.PersonalEmail;
                entity.PermanentAddress = dto.PermanentAddress;
                entity.PresentAddress = dto.PresentAddress;
                entity.Pannumber = dto.PanNumber;
                entity.AadhaarNumber = dto.AadhaarNumber;
                entity.PassportNumber = dto.PassportNumber;
                entity.PlaceOfBirth = dto.PlaceOfBirth;
                entity.Uan = dto.Uan;
                entity.BloodGroup = dto.BloodGroup;
                entity.Citizenship = dto.Citizenship;
                entity.Religion = dto.Religion;
                entity.DrivingLicence = dto.DrivingLicence;
                entity.MaritalStatusId = dto.maritalStatusId;
                entity.MarriageDate = dto.MarriageDate;
                entity.WorkPhone = dto.WorkPhone;
                entity.LinkedInProfile = dto.LinkedInProfile;
                entity.PreviousExperienceText = dto.PreviousExperience;
                entity.ProfilePictureName = dto.profilePicturePath;
                entity.BandGrade = dto.brandGrade;
                entity.EsicNumber = dto.esicNumber;
                entity.Pfnumber = dto.pfNumber;
                entity.EmployeeType = dto.employmentType;
                entity.DateOfJoining = dto.dateofJoining;


                entity.CompanyId = dto.CompanyId;
                entity.RegionId = dto.RegionId;
                entity.UserId = dto.userId;
                entity.ModifiedBy = dto.userId;
                entity.ModifiedAt = DateTime.UtcNow;
                _unitOfWork.Repository<EmployeePersonalDetail>().Update(entity);
                await _unitOfWork.CompleteAsync();

                return MapToDto(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeletePersonalEmailAsync(int id)
        {
            var entity = await _unitOfWork.Repository<EmployeePersonalDetail>().GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.Repository<EmployeePersonalDetail>().Remove(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private PersonalDetailsDto MapToDto(EmployeePersonalDetail entity)
        {
            return new PersonalDetailsDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth,
                genderId = entity.GenderId,
                MobileNumber = entity.MobileNumber,
                PersonalEmail = entity.PersonalEmail,
                PermanentAddress = entity.PermanentAddress,
                PresentAddress = entity.PresentAddress,
                PanNumber = entity.Pannumber,
                AadhaarNumber = entity.AadhaarNumber,
                PassportNumber = entity.PassportNumber,
                PlaceOfBirth = entity.PlaceOfBirth,
                Uan = entity.Uan,
                BloodGroup = entity.BloodGroup,
                Citizenship = entity.Citizenship,
                Religion = entity.Religion,
                DrivingLicence = entity.DrivingLicence,
                maritalStatusId = entity.MaritalStatusId,
                MarriageDate = entity.MarriageDate,
                WorkPhone = entity.WorkPhone,
                LinkedInProfile = entity.LinkedInProfile,
                PreviousExperience = entity.PreviousExperienceText,
                brandGrade = entity.BandGrade,
                esicNumber = entity.EsicNumber,
                pfNumber = entity.Pfnumber,
                employmentType = entity.EmployeeType,
                dateofJoining = entity.DateOfJoining,
                // profilePicture = entity.ProfilePictureName,
                userId = entity.UserId,
                CompanyId = entity.CompanyId,
                RegionId = entity.RegionId,


            };
        }
        #endregion
        #region Digital Business Card
        public async Task<DigitalCardDto> GetDigitalCardAsync(int userId)
        {
            var data = await (
                from u in _context.Users
                join ep in _context.EmployeePersonalDetails on u.UserId equals ep.UserId into epjoin
                from ep in epjoin.DefaultIfEmpty()
                join c in _context.Companies on u.CompanyId equals c.CompanyId into cJoin
                from c in cJoin.DefaultIfEmpty()
                join rg in _context.Regions on u.RegionId equals rg.RegionId into rgJoin
                from rg in rgJoin.DefaultIfEmpty()
                join rm in _context.RoleMasters on u.RoleId equals rm.RoleId into rmJoin
                from rm in rmJoin.DefaultIfEmpty()
                where u.UserId == userId
                select new DigitalCardDto
                {
                    UserID = userId,
                    FullName = u.FullName,
                    Email = u.Email,
                    EmployeeCode = u.EmployeeCode,
                    RoleName = rm.RoleName,
                    CompanyName = c.CompanyName,
                    RegionName = rg.RegionName,
                    MobileNumber = ep.MobileNumber,
                    Location = rg.Country,
                    PersonalEmail = ep.PersonalEmail,
                    LinkedInProfile = ep.LinkedInProfile,
                    ProfilePictureBase64 = ep.ProfilePictureBase64

                }

                ).FirstOrDefaultAsync();

            return data;
        }
        #endregion
        #region Employee Profile Display
        public async Task<EmployeeProfileDto> GetEmployeeProfileAsync(int userId)
        {
            var data = await (
                from u in _context.Users

                join ep in _context.EmployeePersonalDetails
                    on u.UserId equals ep.UserId into epJoin
                from ep in epJoin.DefaultIfEmpty()

                join rm in _context.Users
                    on u.ReportingTo equals rm.UserId into rmJoin
                from rm in rmJoin.DefaultIfEmpty()
                join r in _context.RoleMasters
                    on u.RoleId equals r.RoleId into rJoin
                from r in rJoin.DefaultIfEmpty()
                join sm in _context.ShiftMasters
                    on new { u.CompanyId, u.RegionId }
                    equals new { sm.CompanyId, sm.RegionId } into smJoin
                from sm in smJoin.DefaultIfEmpty()

                    // ⭐ NEW JOIN – Region Table
                join reg in _context.Regions
                    on new { u.CompanyId, u.RegionId }
                    equals new { reg.CompanyId, reg.RegionId } into regJoin
                from reg in regJoin.DefaultIfEmpty()


                where u.UserId == userId

                select new EmployeeProfileDto
                {
                    EmployeeCode = u.EmployeeCode,
                    FullName = u.FullName,
                    Email = u.Email,

                    Phone = ep.MobileNumber,

                    BandGrade = ep.BandGrade,
                    EsicNumber = ep.EsicNumber,
                    PFNumber = ep.Pfnumber,
                    UAN = ep.Uan,

                    ReportingManager = rm.FullName,
                    DateOfJoining = ep.DateOfJoining,

                    Rolename = r.RoleName,
                    EmployeeType = ep.EmployeeType,

                    ServiceStatus = u.Status,
                    Location = reg.RegionName,

                    ShiftName = sm.ShiftName,
                    SkypeId = ep.LinkedInProfile
                }
            ).FirstOrDefaultAsync();

            return data;
        }
        #endregion
        #region Employee Emergency Contact
        public async Task<IEnumerable<EmployeeEmergencyContactDto>> GetByUseremergencyContactAsync(int userId, int companyId, int regionId)
        {
            return await _context.EmployeeEmergencyContacts
                .Include(x => x.Relationship)
                .Where(x =>
                            x.UserId == userId &&
                            x.CompanyId == companyId &&
                            x.RegionId == regionId)
                .Select(x => new EmployeeEmergencyContactDto
                {
                    EmergencyContactId = x.EmergencyContactId,
                    UserId = x.UserId,
                    ContactName = x.ContactName,
                    RelationshipId = x.RelationshipId,
                    Relationship = x.Relationship.RelationshipName,
                    PhoneNumber = x.PhoneNumber,
                    AlternatePhone = x.AlternatePhone,
                    Email = x.Email,
                    Address = x.Address,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId

                })
                .ToListAsync();
        }

        public async Task<EmployeeEmergencyContactDto?> GetByIdempEmergencyAsync(int emergencyContactId)
        {
            return await _context.EmployeeEmergencyContacts
                .Include(x => x.Relationship)
                .Where(x => x.EmergencyContactId == emergencyContactId)
                .Select(x => new EmployeeEmergencyContactDto
                {
                    EmergencyContactId = x.EmergencyContactId,
                    UserId = x.UserId,
                    ContactName = x.ContactName,
                    RelationshipId = x.RelationshipId,
                    Relationship = x.Relationship.RelationshipName,
                    PhoneNumber = x.PhoneNumber,
                    AlternatePhone = x.AlternatePhone,
                    Email = x.Email,
                    Address = x.Address,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,

                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeEmergencyContactDto> AddempEmergencyAsync(EmployeeEmergencyContactDto dto)
        {
            var entity = new EmployeeEmergencyContact
            {
                UserId = dto.UserId,
                ContactName = dto.ContactName,
                RelationshipId = dto.RelationshipId,
                PhoneNumber = dto.PhoneNumber,
                AlternatePhone = dto.AlternatePhone,
                Email = dto.Email,
                Address = dto.Address,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedBy = dto.UserId.ToString(),
                CreatedDate = DateTime.UtcNow,

            };

            _context.EmployeeEmergencyContacts.Add(entity);
            await _context.SaveChangesAsync();

            dto.EmergencyContactId = entity.EmergencyContactId;
            dto.CreatedDate = entity.CreatedDate;

            return dto;
        }

        public async Task<EmployeeEmergencyContactDto?> UpdateempEmergencyAsync(EmployeeEmergencyContactDto dto)
        {
            var entity = await _context.EmployeeEmergencyContacts
                .FirstOrDefaultAsync(x => x.EmergencyContactId == dto.EmergencyContactId);

            if (entity == null)
                return null;

            entity.ContactName = dto.ContactName;
            entity.RelationshipId = dto.RelationshipId;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.AlternatePhone = dto.AlternatePhone;
            entity.Email = dto.Email;
            entity.Address = dto.Address;
            entity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();


            return dto;
        }

        public async Task<bool> DeleteempEmergencyAsync(int emergencyContactId)
        {
            var entity = await _context.EmployeeEmergencyContacts
                .FirstOrDefaultAsync(x => x.EmergencyContactId == emergencyContactId);

            if (entity == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        public async Task<List<EmployeeDto>> GetEmployeesByCompanyAndRegionAsync(int companyId, int regionId)
        {
            var data = await (
                from u in _context.Users

                

                where u.CompanyId == companyId && u.RegionId == regionId
                      && u.Status == "Active" // Optional: only active employees

                select new EmployeeDto
                {
                    UserId = u.UserId,
                    EmployeeCode = u.EmployeeCode,
                    EmployeeName = u.FullName,
                    Email = u.Email,
                  
                    Status = u.Status
                }
            ).ToListAsync();

            return data;
        }

    }
}
