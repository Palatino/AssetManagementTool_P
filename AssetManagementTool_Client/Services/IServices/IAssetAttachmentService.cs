using AssetManagementTool_Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_Client.Services.IServices
{
    public interface IAssetAttachmentService
    {
        public Task<CommentAttachmentDTO> CreateCommentAttachment(NewCommentAttachmentDTO newCommentAttachmentDTO);
        public Task<bool> DeleteCommentAttachment(int id);
        public Task<ImageAttachmentDTO> CreateImageAttachment(NewImageAttachmentDTO newImageAttachmentDTO);
        public Task<bool> DeleteImageAttachment(int id);
        public Task<FileAttachmentDTO> CreateFileAttachment(NewFileAttachmentDTO newFileAttachmentDTO);
        public Task<string> GetFile(int id);
        public Task<bool> DeleteFileAttachment(int id);
    }
}
