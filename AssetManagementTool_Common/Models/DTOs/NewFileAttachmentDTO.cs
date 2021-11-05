using AssetManagementTool_Common.Models.BussinesModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class NewFileAttachmentDTO
    {
        [Required]
        public int AssetId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public FileType FileType { get; set; }
        public string FileExtension { get; set; }
        public string ElementOwner { get; set; } = null;
        public byte[] FileByteArray { get; set; }
        public string Description { get; set; }
        public string AddedBy { get; set; }

    }
}
