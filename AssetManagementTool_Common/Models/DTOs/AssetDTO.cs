using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class AssetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string IFCBlobLink { get; set; }
        public IEnumerable<CommentAttachmentDTO> Comments { get; set; }
        public IEnumerable<ImageAttachmentDTO> Images { get; set; }
        public IEnumerable<FileAttachmentDTO> Files { get; set; }
    }
}
