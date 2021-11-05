using AssetManagementTool_API.Data.Repositories.IRepositories;
using AssetManagementTool_API.Services;
using AssetManagementTool_API.Services.IServices;
using AssetManagementTool_Common.Models.BussinesModels;
using AssetManagementTool_Common.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly AssetManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        public AttachmentRepository(AssetManagementDbContext context, IMapper mapper, IBlobService blobService)
        {
            _blobService = blobService;
            _context = context;
            _mapper = mapper;
        }
        public async Task<CommentAttachmentDTO> CreateCommentAttachment(NewCommentAttachmentDTO newCommentAttachmentDTO)
        {
            CommentAttachment newComment = new CommentAttachment();
            newComment.AssetId = newCommentAttachmentDTO.AssetId;
            newComment.Content = newCommentAttachmentDTO.Content;
            newComment.ElementOwner = newCommentAttachmentDTO.ElementOwner;
            newComment.AddedBy = newCommentAttachmentDTO.AddedBy;

            _context.AssetsComments.Add(newComment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentAttachment, CommentAttachmentDTO>(newComment);
        }

        public async Task<FileAttachmentDTO> CreateFileAttachment(NewFileAttachmentDTO newFileAttachmentDTO)
        {
            FileAttachment newFile = new FileAttachment();
            newFile.AssetId = newFileAttachmentDTO.AssetId;
            newFile.Description = newFileAttachmentDTO.Description;
            newFile.FileType = newFileAttachmentDTO.FileType;
            newFile.ElementOwner = newFileAttachmentDTO.ElementOwner;
            newFile.Name = newFileAttachmentDTO.Name;
            newFile.AddedBy = newFileAttachmentDTO.AddedBy;

            newFile.FileBlobName = await _blobService.UploadContentBlobAsync(
            newFileAttachmentDTO.FileByteArray,
            Guid.NewGuid().ToString() + newFileAttachmentDTO.FileExtension,
            BlobContainerNames.FilesContainer);

            if (string.IsNullOrWhiteSpace(newFile.FileBlobName))
            {
                throw new Exception("Could not upload image");
            }

            _context.AssetsFiles.Add(newFile);
            await _context.SaveChangesAsync();

            return _mapper.Map<FileAttachment, FileAttachmentDTO>(newFile);

        }

        /// <summary>
        /// Get link to file open with token valid for 5 minutes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetFileLink(int id)
        {
            var fileInDb =  await _context.AssetsFiles.FindAsync(id);
            if(fileInDb is null)
            {
                return null;
            }

            var link = _blobService.GetBlobURL(fileInDb.FileBlobName, BlobContainerNames.FilesContainer, 5);
            return link;
        }
        public async Task<ImageAttachmentDTO> CreateImageAttachment(NewImageAttachmentDTO newImageAttachmentDTO)
        {
            ImageAttachment newImage = new ImageAttachment();
            newImage.AssetId = newImageAttachmentDTO.AssetId;
            newImage.ElementOwner = newImageAttachmentDTO.ElementOwner;
            newImage.ImageBlobName = await _blobService.UploadContentBlobAsync(
                newImageAttachmentDTO.Image,
                Guid.NewGuid().ToString() + newImageAttachmentDTO.Format,
            BlobContainerNames.ImagesContainer);
            newImage.AddedBy = newImageAttachmentDTO.AddedBy;

            if (string.IsNullOrWhiteSpace(newImage.ImageBlobName))
            {
                throw new Exception("Could not upload image");
            }
            _context.AssetsImages.Add(newImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<ImageAttachment, ImageAttachmentDTO>(newImage);
        }

        public async Task<int> DeleteCommentAttachment(int id)
        {
            var commentInDb = await _context.AssetsComments.FindAsync(id);

            if(commentInDb is null)
            {
                return 0;
            }

            _context.AssetsComments.Remove(commentInDb);
            var changes =await _context.SaveChangesAsync();
            return changes;
        }

        public async Task<int> DeleteFileAttachment(int id)
        {
            var fileInDb = await _context.AssetsFiles.FindAsync(id);

            if (fileInDb is null)
            {
                return 0;
            }

            await _blobService.DeleteBlobAsync(fileInDb.FileBlobName, BlobContainerNames.FilesContainer);
            _context.AssetsFiles.Remove(fileInDb);
            var changes = await _context.SaveChangesAsync();
            return changes;
        }

        public async Task<int> DeleteImageAttachment(int id)
        {
            var imageInDb = await _context.AssetsImages.FindAsync(id);

            if (imageInDb is null)
            {
                return 0;
            }

            await _blobService.DeleteBlobAsync(imageInDb.ImageBlobName, BlobContainerNames.ImagesContainer);
            _context.AssetsImages.Remove(imageInDb);
            var changes = await _context.SaveChangesAsync();
            return changes;
        }

        public Task<IEnumerable<CommentAttachmentDTO>> GetAssetComments(int assetId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileAttachmentDTO>> GetAssetFiles(int assetId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageAttachmentDTO>> GetAssetImages(int assetId)
        {
            throw new NotImplementedException();
        }


    }
}
