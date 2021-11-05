using AssetManagementTool_Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data.Repositories.IRepositories
{
    public interface IAttachmentRepository
    {
        public Task<ImageAttachmentDTO> CreateImageAttachment(NewImageAttachmentDTO newImageAttachmentDTO);
        public Task<int> DeleteImageAttachment(int id);
        public Task<FileAttachmentDTO> CreateFileAttachment(NewFileAttachmentDTO newFileAttachmentDTO);
        public Task<string> GetFileLink(int id);
        public Task<int> DeleteFileAttachment(int id);
        public Task<CommentAttachmentDTO> CreateCommentAttachment(NewCommentAttachmentDTO newCommentAttachmentDTO);
        public Task<int> DeleteCommentAttachment(int id);

        public Task<IEnumerable<ImageAttachmentDTO>> GetAssetImages(int assetId);
        public Task<IEnumerable<FileAttachmentDTO>> GetAssetFiles(int assetId);
        public Task<IEnumerable<CommentAttachmentDTO>> GetAssetComments(int assetId);

    }
}
