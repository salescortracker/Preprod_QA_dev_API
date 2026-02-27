using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAssetStatusService
    {
        Task<List<AssetStatusDto>> GetAllAsync(int companyId, int regionId);
        Task<int> CreateAsync(AssetStatusDto dto);
        Task<bool> UpdateAsync(AssetStatusDto dto);
        Task<bool> DeleteAsync(int assetStatusId);
    }
}
