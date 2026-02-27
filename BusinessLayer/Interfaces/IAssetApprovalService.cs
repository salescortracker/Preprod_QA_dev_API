using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAssetApprovalService
    {
        Task<List<AssetApprovalDto>> GetPendingAssetsForManagerAsync(int managerUserId);

        Task<bool> ApproveOrRejectAssetAsync(
            int assetId,
            int managerUserId,
            string action   // "Approve" or "Reject"
        );

    }
}
