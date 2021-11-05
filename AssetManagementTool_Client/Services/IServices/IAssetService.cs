using AssetManagementTool_Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_Client.Services.IServices
{
    public interface IAssetService
    {
        public Task<IEnumerable<AssetDTO>> GetAssets();
        public Task<AssetDTO> GetAsset(int id);
        public Task<bool> DeleteAsset(int id);
        public Task<AssetDTO> CreateAsset(NewAssetDTO newAsset);
        public Task<IEnumerable<string>> GetAssetNames();
    }
}
