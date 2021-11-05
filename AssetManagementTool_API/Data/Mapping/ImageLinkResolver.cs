using AssetManagementTool_API.Services;
using AssetManagementTool_API.Services.IServices;
using AssetManagementTool_Common.Models.BussinesModels;
using AssetManagementTool_Common.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data.Mapping
{


    public class ImageLinkResolver : IValueResolver<ImageAttachment, ImageAttachmentDTO, string>
    {
        private IBlobService _blobService;

        public ImageLinkResolver(IBlobService blobService)
        {
            _blobService = blobService;
        }
        public string Resolve(ImageAttachment source, ImageAttachmentDTO destination, string destMember, ResolutionContext context)
        {
            return _blobService.GetBlobURL(source.ImageBlobName, BlobContainerNames.ImagesContainer);
        }
    }
}
