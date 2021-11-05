using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagementTool_Common.Models.DTOs;

namespace AssetManagementTool_API.Data.Repositories.IRepositories
{
    public interface IAssetRepository
    {
        public Task<AssetDTO> GetAsset(int id);
        public Task<IEnumerable<AssetDTO>> GetAssets();
        public Task<AssetDTO> CreateAsset(NewAssetDTO newAsset);
        public Task<AssetDTO> UpdateAsset(AssetDTO updatedAsset);
        public Task<int> DeleteAsset(int id);
        public Task<IEnumerable<string>> GetAssetNames();


    }
}
