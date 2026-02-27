using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRecruitmentService
    {
        ///resume upload
        Task<int> SaveCandidateAsync(CandidateDto dto);

        Task<IEnumerable<object>> GetCandidatesAsync(int userId, int companyId, int regionId);

        Task<bool> MoveStageAsync(int candidateId, int stageId);

        Task<bool> DeleteCandidateAsync(int candidateId);


        Task<CandidateDto?> GetCandidateByIdAsync(int candidateId);
        Task<bool> UpdateCandidateAsync(CandidateDto dto);

        Task<IEnumerable<object>> GetReferenceUsersAsync(int companyId, int regionId);

        /////////screening
        Task<IEnumerable<object>> GetRecruitersAsync(int companyId, int regionId);

        Task<IEnumerable<object>> GetScreeningCandidatesTopTableAsync(
   int companyId,
   int regionId,
   string department,
   string designation
);
        Task<bool> SaveCandidateScreeningAsync(CandidateScreeningDto dto);

        Task<IEnumerable<CandidateScreeningDto>> GetScreeningRecordsAsync(int userId, int companyId, int regionId);

        Task<bool> UpdateCandidateScreeningAsync(CandidateScreeningDto dto);

        //////////Interview

        Task<IEnumerable<object>> GetScreeningCandidatesTopTableInterviewAsync(
 int companyId,
 int regionId,
 string department,
 string designation
);
        Task<bool> SaveCandidateInterviewAsync(CandidateInterviewDto dto);

        Task<IEnumerable<CandidateInterviewDto>> GetInterviewRecordsAsync(int userId,
            int companyId,
            int regionId
        );
        Task<bool> UpdateCandidateInterviewAsync(CandidateInterviewDto dto);

        // Appointment Screen
        Task<IEnumerable<CandidateAppointmentDto>> GetAppointmentsForInterviewerAsync(
            int companyId,
            int regionId,
            int interviewerId
        );
        Task<object?> GetAppointmentCandidateDetailsAsync(int candidateId);
    }
}
