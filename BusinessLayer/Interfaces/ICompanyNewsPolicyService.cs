using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    
    using global::BusinessLayer.DTOs;

    namespace BusinessLayer.Interfaces
    {
        public interface ICompanyNewsPolicyService
        {
            // ================= COMPANY NEWS =================

            Task<IEnumerable<CompanyNewsMasterDto>> GetAllNewsAsync(int userId);

            Task<IEnumerable<CompanyNewsMasterDto>> GetTodayNewsAsync(int userId);

            Task<CompanyNewsMasterDto?> GetNewsByIdAsync(int id, int userId);

            Task<CompanyNewsMasterDto> AddNewsAsync(CompanyNewsMasterDto dto);

            Task<CompanyNewsMasterDto> UpdateNewsAsync(int id, CompanyNewsMasterDto dto);

            Task<bool> DeleteNewsAsync(int id, int userId);


            // ================= COMPANY POLICIES =================

            Task<IEnumerable<CompanyPolicyMasterDto>> GetAllPoliciesAsync(int userId);

            Task<IEnumerable<CompanyPolicyMasterDto>> GetTodayPoliciesAsync(int userId);

            Task<CompanyPolicyMasterDto?> GetPolicyByIdAsync(int id, int userId);

            Task<CompanyPolicyMasterDto> AddPolicyAsync(CompanyPolicyMasterDto dto);

            Task<CompanyPolicyMasterDto> UpdatePolicyAsync(int id, CompanyPolicyMasterDto dto);

            Task<bool> DeletePolicyAsync(int id, int userId);
        }
    }
}
