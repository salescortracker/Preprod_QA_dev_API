using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICertificationTypeService
    {
        Task<ApiResponse<IEnumerable<CertificationTypeDto>>> GetAllAsync(
            int userId, int regionId);

        Task<ApiResponse<CertificationTypeDto?>> GetByIdAsync(int id);
        Task<ApiResponse<CertificationTypeDto>> CreateAsync(CreateUpdateCertificationTypeDto dto);
        Task<ApiResponse<CertificationTypeDto>> UpdateAsync( CreateUpdateCertificationTypeDto dto);
        Task<ApiResponse<object>> DeleteAsync(int id);
        Task<ApiResponse<IEnumerable<CertificationTypeDto>>> GetCmpregionAllAsync(
          int companyId, int regionId);

        Task<ApiResponse<(int inserted, int duplicates, int failed)>> BulkInsertAsync(
            IEnumerable<CreateUpdateCertificationTypeDto> items, int createdBy);
    }
}
