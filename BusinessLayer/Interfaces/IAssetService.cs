using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAssetService
    {
        Task<List<AssetDto>> GetAllAssetsAsync();
        Task<List<AssetDto>> GetAssetsByUserIdAsync(int userId);
        Task<int> CreateAssetAsync(AssetDto assetDto);
        Task<bool> UpdateAssetAsync(AssetDto assetDto);
        Task<bool> DeleteAssetAsync(int assetId);
        Task<List<AssetStatusDto>> GetAllAssetStatusesAsync();

        Task<List<EmployeeDto>> GetAllEmployeesAsync();

    }
}
