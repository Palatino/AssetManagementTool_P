
using AssetManagementTool_API.Data.Repositories.IRepositories;
using AssetManagementTool_API.Services;
using AssetManagementTool_API.Services.IServices;
using AssetManagementTool_Common.Models.BussinesModels;
using AssetManagementTool_Common.Models.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AssetManagementDbContext _context;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IAttachmentRepository _attachmentRepo;

        public AssetRepository(AssetManagementDbContext context, IMapper mapper, IBlobService blobService , IAttachmentRepository attachementRepo)
        {
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
            _attachmentRepo = attachementRepo;
        }
        public async Task<AssetDTO> CreateAsset(NewAssetDTO newAssetDTO)
        {
            var newAsset = new Asset();

            newAsset.Name = newAssetDTO.Name;
            newAsset.Latitude = newAssetDTO.Latitude;
            newAsset.Longitude = newAssetDTO.Longitude;
            newAsset.IFCBlobName = await _blobService.UploadContentBlobAsync(
                newAssetDTO.IFCFile, 
                newAssetDTO.Name + ".ifc", 
                BlobContainerNames.IFCContainer);
            newAsset.AddedBy = newAssetDTO.AddedBy;
            _context.Assets.Add(newAsset);
            await _context.SaveChangesAsync();
            return _mapper.Map<Asset, AssetDTO>(newAsset);
        }

        public async Task<int> DeleteAsset(int id)
        {
            var AssetInDb = await _context.Assets
                                .Include(a => a.Comments)
                                .Include(a => a.Images)
                                .Include(a => a.Files)
                                .SingleOrDefaultAsync(a=>a.Id==id);

            if (AssetInDb is null)
            {
                return 0;
            }

            await DeleteAssetAttachments(AssetInDb);
            await _blobService.DeleteBlobAsync(AssetInDb.IFCBlobName, BlobContainerNames.IFCContainer);
            _context.Assets.Remove(AssetInDb);
            var changes = await _context.SaveChangesAsync();
            return changes;
        }

        private async Task DeleteAssetAttachments(Asset asset)
        {
            foreach(var comment in asset.Comments.ToList())
            {
                await _attachmentRepo.DeleteCommentAttachment(comment.Id);
            }
            foreach (var image in asset.Images.ToList())
            {
                await _attachmentRepo.DeleteImageAttachment(image.Id);
            }
            foreach (var file in asset.Files.ToList())
            {
                await _attachmentRepo.DeleteFileAttachment(file.Id);
            }

        }

        public async Task<AssetDTO> GetAsset(int id)
        {
            var AssetInDb = await _context.Assets
                .Where(a=>a.Id==id)
                .Include(a=>a.Images)
                .Include(a=>a.Comments)
                .Include(a=>a.Files)
                .SingleOrDefaultAsync();
            if(AssetInDb is null)
            {
                return null;
            }

            var assetDTO = _mapper.Map<Asset, AssetDTO>(AssetInDb);
            return assetDTO;
        }

        public async  Task<IEnumerable<AssetDTO>> GetAssets()
        {
            var AssetsInDb = await _context.Assets.ToListAsync();
            var assetsDTOs = _mapper.Map<IEnumerable<Asset>, IEnumerable<AssetDTO>>(AssetsInDb);
            return assetsDTOs;
        }

        public async Task<IEnumerable<string>> GetAssetNames()
        {
            var AssetsNamesInDb = await _context.Assets.Select(a=>a.Name).ToListAsync();
            return AssetsNamesInDb;
        }
        //End point to update asset metadata and IFC file only, no attachments
        public async Task<AssetDTO> UpdateAsset(AssetDTO updatedAssetDTO)
        {
            var AssetInDb = await _context.Assets.FindAsync(updatedAssetDTO.Id);
            if (AssetInDb is null)
            {
                throw new KeyNotFoundException();
            }

            AssetInDb.Name = updatedAssetDTO.Name;
            AssetInDb.Latitude = updatedAssetDTO.Latitude;
            AssetInDb.Longitude = updatedAssetDTO.Longitude;

            await _context.SaveChangesAsync();

            return _mapper.Map<Asset, AssetDTO>(AssetInDb);
        }
    }
}
