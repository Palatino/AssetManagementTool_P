using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class NewCommentAttachmentDTO
    {
        [Required]
        public string Content { get; set; }
        public int AssetId { get; set; }
        public string ElementOwner { get; set; } = null;
        public string AddedBy { get; set; }
    }
}
