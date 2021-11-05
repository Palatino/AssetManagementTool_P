
using AssetManagementTool_Common.Models;
using AssetManagementTool_Common.Models.BussinesModels;
using AssetManagementTool_Common.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Asset, AssetDTO>()
                .ForMember(a => a.IFCBlobLink, m => m.MapFrom<IFCLinkResolver>());
            CreateMap<AssetDTO, Asset>();

            CreateMap<ImageAttachment, ImageAttachmentDTO>()
                .ForMember(i => i.ImageBlobLink, m => m.MapFrom<ImageLinkResolver>());
            CreateMap<ImageAttachmentDTO, ImageAttachment>();

            CreateMap<FileAttachment, FileAttachmentDTO>().ReverseMap();
            CreateMap<CommentAttachment, CommentAttachmentDTO>().ReverseMap();

        }
    }
}
