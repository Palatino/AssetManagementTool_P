using AssetManagementTool_Common.Models;
using AssetManagementTool_Common.Models.BussinesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class FileAttachmentDTO
    {
        public int Id { get; set; }
        [JsonIgnore]
        public AssetDTO Asset { get; set; }
        public int AssetId { get; set; }
        public string ElementOwner { get; set; } = null;
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
        public string FileBlobLink { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FileType FileType { get; set; }
    }
}
