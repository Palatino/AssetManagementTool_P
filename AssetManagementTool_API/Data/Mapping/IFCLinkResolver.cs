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

    public class IFCLinkResolver : IValueResolver<Asset, AssetDTO, string>
    {
        private IBlobService _blobService;

        public IFCLinkResolver(IBlobService blobService)
        {
            _blobService = blobService;
        }
        public string Resolve(Asset source, AssetDTO destination, string destMember, ResolutionContext context)
        {
            return _blobService.GetBlobURL(source.IFCBlobName, BlobContainerNames.IFCContainer);
        }
    }
}
