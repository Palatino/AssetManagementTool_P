using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class NewImageAttachmentDTO
    {
        [Required]
        public int AssetId { get; set; }
        public byte[] Image { get; set; }
        public string ElementOwner { get; set; } = null;
        public string Format { get; set; }
        public string AddedBy { get; set; }

    }
}
