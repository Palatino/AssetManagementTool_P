using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class ImageAttachmentDTO
    {
        public int Id { get; set; }
        [JsonIgnore]
        public AssetDTO Asset { get; set; }
        public int AssetId { get; set; }
        public string ElementOwner { get; set; } = null;
        public string ImageBlobLink { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
