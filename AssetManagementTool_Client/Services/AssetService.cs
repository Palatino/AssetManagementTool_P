using AssetManagementTool_Client.Services.IServices;
using AssetManagementTool_Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AssetManagementTool_Client.Services
{
    public class AssetService : IAssetService
    {
        private HttpClient _client;
        public AssetService(HttpClient client)
        {
            _client = client;

        }

        public async Task<AssetDTO> CreateAsset(NewAssetDTO newAssetDTO)
        {
            var response = await _client.PostAsJsonAsync<NewAssetDTO>("/api/assets",newAssetDTO);
            var responseString = await response.Content.ReadAsStringAsync();
            var assetDTO = JsonSerializer.Deserialize<AssetDTO>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return assetDTO;
        }

        public async Task<bool> DeleteAsset(int id)
        {
            var response = await _client.DeleteAsync($"/api/assets/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
            return false;
        }

        public async Task<AssetDTO> GetAsset(int id)
        {
            var assetDTO = await _client.GetFromJsonAsync<AssetDTO>($"/api/assets/{id}");
            return assetDTO;
        }

        public async Task<IEnumerable<AssetDTO>> GetAssets()
        {
            var assetsDTO = await _client.GetFromJsonAsync<IEnumerable<AssetDTO>>("/api/assets");
            return assetsDTO;
        }

        public async Task<IEnumerable<string>> GetAssetNames()
        {
            var names = await _client.GetFromJsonAsync<IEnumerable<string>>("/api/assets/names");
            return names;
        }
    }
}
