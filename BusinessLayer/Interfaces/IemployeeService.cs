using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IemployeeService
    {
        Task<IEnumerable<EmployeeCertificationDto>> getAllEmpCertAsync();
        Task<IEnumerable<EmployeeCertificationDto>> getByUserIdEmpCertAsync(int userId);
        Task<EmployeeCertificationDto?> getByIdEmpCertAsync(int certificationId);
        Task<int> addEmpCertAsync(EmployeeCertificationDto model);
        Task<bool> updateEmpCertAsync(EmployeeCertificationDto model);
        Task<bool> deleteEmpCertAsync(int certificationId);

        Task<IEnumerable<CertificationTypeDto>> GetCertificationTypesAsync();
        Task<IEnumerable<EmployeeEducationDto>> getByUserIdempEduAsync(int userId);
        Task<EmployeeEducationDto?> getByIdEmpEduAsync(int educationId);
        Task<int> addEmpEduAsync(EmployeeEducationDto model);
        Task<bool> updateEmpEduAsync(EmployeeEducationDto model);
        Task<bool> deleteEmpEduAsync(int educationId);
        Task<IEnumerable<EmployeeEducationDto>> getAllEmpEduAsync();
        Task<IEnumerable<ModeOfStudyDto>> GetModeOfStudyListAsync();

        Task<IEnumerable<EmployeeJobHistoryDto>> getAllempJobAsync();
        Task<IEnumerable<EmployeeJobHistoryDto>> getByUserIdEmpJobAsync(int userId);
        Task<EmployeeJobHistoryDto?> getByIdEmpJobAsync(int id);
        Task<int> addEmpJobAsync(EmployeeJobHistoryDto model);
        Task<bool> updateEmpJobAsync(EmployeeJobHistoryDto model);
        Task<bool> deleteEmpJobAsync(int id);

        Task<List<EmployeeImmigrationDto>> GetAllImmigrationAsync();
        Task<EmployeeImmigrationDto> GetByIdImmigrationAsync(int id);
        Task<bool> CreateImmigrationAsync(EmployeeImmigrationDto dto);
        Task<bool> UpdateImmigrationAsync(EmployeeImmigrationDto dto);
        Task<bool> DeleteImmigrationAsync(int id);

        Task<List<VisaTypeDto>> GetVisaTypesAsync();
        Task<List<WorkAuthStatusDto>> GetStatusListAsync();

        Task<IEnumerable<DocumentTypeDto>> GetActiveDocumentTypesAsync();
        Task<IEnumerable<EmployeeDocumentDto>> getAllempDocAsync();
        Task<IEnumerable<EmployeeDocumentDto>> getByUserIdempDocAsync(int userId);
        Task<EmployeeDocumentDto?> getByIdempDocAsync(int id);
        Task<int> addempDocAsync(EmployeeDocumentDto model);
        Task<bool> updateempDocAsync(EmployeeDocumentDto model);
        Task<bool> deleteempDocAsync(int id);

        Task<IEnumerable<EmployeeFormDto>> getAllempFormAsync();
        Task<IEnumerable<EmployeeFormDto>> getByUserIdempFormAsync(int userId);
        Task<EmployeeFormDto?> getByIdempFormAsync(int id);
        Task<int> addempFormAsync(EmployeeFormDto model);
        Task<bool> updateempFormAsync(EmployeeFormDto model);
        Task<bool> deleteempFormAsync(int id);

        Task<IEnumerable<EmployeeLetterDto>> getAllempLetterAsync();
        Task<IEnumerable<EmployeeLetterDto>> getByUserIdempLetterAsync(int userId);
        Task<EmployeeLetterDto?> getByIdempLetterAsync(int id);
        Task<int> addempLetterAsync(EmployeeLetterDto model);
        Task<bool> updateempLetterAsync(EmployeeLetterDto model);
        Task<bool> deleteempLetterAsync(int id);

        Task<IEnumerable<EmployeeBankDetailsDto>> getAllempBankAsync();
        Task<EmployeeBankDetailsDto?> getByIdempBankAsync(int id);
        Task<bool> addempBankAsync(EmployeeBankDetailsDto dto);
        Task<bool> updateempBankAsync(EmployeeBankDetailsDto dto);
        Task<bool> deleteempBankAsync(int id);

        Task<IEnumerable<EmployeeDdlistDto>> getAllempDDAsync();
        Task<EmployeeDdlistDto?> getByIdempDDAsync(int id);
        Task<bool> addempDDAsync(EmployeeDdlistDto dto);
        Task<bool> updateempDDAsync(EmployeeDdlistDto dto);
        Task<bool> deleteempDDAsync(int id);

        Task<List<EmployeeW4Dto>> getAllempW4Async();
        Task<EmployeeW4Dto?> getByIdempW4Async(int id);
        Task<bool> addempW4Async(EmployeeW4Dto dto);
        Task<bool> updateempW4Async(EmployeeW4Dto dto);
        Task<bool> deleteempW4Async(int id);

        Task<IEnumerable<PersonalDetailsDto>> GetAllPersonalEmailAsync();
        Task<PersonalDetailsDto?> GetByIdPersonalEmailAsync(int id);
        Task<IEnumerable<PersonalDetailsDto>> SearchPersonalEmailAsync(object filter);
        Task<PersonalDetailsDto> AddPersonalEmailAsync(PersonalDetailsDto dto);
        Task<PersonalDetailsDto> UpdateempPersonalAsync(PersonalDetailsDto dto);

        Task<bool> DeletePersonalEmailAsync(int id);

        Task<PersonalDetailsDto?> GetByUserIdempProfileAsync(int userId);

        Task<DigitalCardDto> GetDigitalCardAsync(int userId);

        Task<EmployeeProfileDto?> GetEmployeeProfileAsync(int userId);

        Task<bool> deleteempFamilyAsync(int familyId);
        Task<bool> updateempFamilyAsync(EmployeeFamilyDto model);
        Task<int> addempFamilyAsync(EmployeeFamilyDto model);
        Task<EmployeeFamilyDto?> getByIdempFamilyAsync(int familyId);
        Task<IEnumerable<EmployeeFamilyDto>> getByUserIdempFamilyAsync(int userId);
        Task<IEnumerable<EmployeeFamilyDto>> getAllempFamilyAsync();
        Task<IEnumerable<EmployeeEmergencyContactDto>> GetAllempEmerAsync();
        Task<IEnumerable<EmployeeEmergencyContactDto>> GetByUserIdempEmerAsync(int userId);
        Task<EmployeeEmergencyContactDto?> GetByIdempEmerAsync(int emergencyContactId);
        Task<int> AddempEmerAsync(EmployeeEmergencyContactDto model);
        Task<bool> UpdateempEmerAsync(EmployeeEmergencyContactDto model);
        Task<bool> DeleteempEmerAsync(int emergencyContactId);
        Task<IEnumerable<EmployeeReferenceDto>> getAllempRefAsync();

        Task<IEnumerable<EmployeeReferenceDto>> getByUserIdempRefAsync(int userId);

        Task<EmployeeReferenceDto?> getByIdempRefAsync(int referenceId);

        Task<int> addempRefAsync(EmployeeReferenceDto model);
        Task<bool> updateempRefAsync(EmployeeReferenceDto model);
        Task<bool> deleteempRefAsync(int referenceId);

        Task<IEnumerable<EmployeeEmergencyContactDto>> GetByUseremergencyContactAsync(int userId, int companyId, int regionId);
        Task<EmployeeEmergencyContactDto?> GetByIdempEmergencyAsync(int emergencyContactId);
        Task<EmployeeEmergencyContactDto> AddempEmergencyAsync(EmployeeEmergencyContactDto contactDto);
        Task<EmployeeEmergencyContactDto?> UpdateempEmergencyAsync(EmployeeEmergencyContactDto contactDto);
        Task<bool> DeleteempEmergencyAsync(int emergencyContactId);
    }
}
