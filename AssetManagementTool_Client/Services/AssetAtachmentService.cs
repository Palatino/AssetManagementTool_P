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
    public class AssetAtachmentService : IAssetAttachmentService
    {
        private HttpClient _client;
        public AssetAtachmentService(HttpClient client)
        {
            _client = client;
        }
        public async Task<CommentAttachmentDTO> CreateCommentAttachment(NewCommentAttachmentDTO newCommentAttachmentDTO)
        {
            var response = await _client.PostAsJsonAsync("/api/comments", newCommentAttachmentDTO);
            var responseString = await response.Content.ReadAsStringAsync();
            var commentDTO = JsonSerializer.Deserialize<CommentAttachmentDTO>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return commentDTO;
        }

        public async Task<bool> DeleteCommentAttachment(int id)
        {
            var response = await _client.DeleteAsync($"/api/comments/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
            return false;
        }
        public async Task<ImageAttachmentDTO> CreateImageAttachment(NewImageAttachmentDTO newImageAttachmentDTO)
        {
            var response = await _client.PostAsJsonAsync("/api/images", newImageAttachmentDTO);
            var responseString = await response.Content.ReadAsStringAsync();
            var imageDTO = JsonSerializer.Deserialize<ImageAttachmentDTO>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return imageDTO;
        }
        public async Task<bool> DeleteImageAttachment(int id)
        {
            var response = await _client.DeleteAsync($"/api/images/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
            return false;
        }

        public async Task<FileAttachmentDTO> CreateFileAttachment(NewFileAttachmentDTO newFileAttachmentDTO)
        {
            var response = await _client.PostAsJsonAsync("/api/files", newFileAttachmentDTO);
            var responseString = await response.Content.ReadAsStringAsync();
            var fileDTO = JsonSerializer.Deserialize<FileAttachmentDTO>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return fileDTO;
        }
        public async Task<bool> DeleteFileAttachment(int id)
        {
            var response = await _client.DeleteAsync($"/api/files/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
            return false;
        }
        public async Task<string> GetFile(int id)
        {
            var response = await _client.GetAsync($"/api/files/{id}");
            var link = await response.Content.ReadAsStringAsync();
            return link;
        }
    }
}
