using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICompanyPolicyService
    {
        Task<List<CompanyPolicyDto>> GetAllPolicies();
        Task<CompanyPolicyDto?> GetPolicyById(int id);
        Task<CompanyPolicyDto> CreatePolicy(CompanyPolicyDto dto);
        Task<CompanyPolicyDto?> UpdatePolicy(CompanyPolicyDto dto);
        Task<bool> DeletePolicy(int id);

        // Dropdown category list
        Task<List<CreateUpdatePolicyCategoryDto>> GetPolicyCategoryDropdown(int companyId, int regionId);
    }
}
