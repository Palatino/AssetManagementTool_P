using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.BussinesModels
{
    public class FileAttachment : ElementAttachment
    {
        public string FileBlobName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FileType FileType { get; set; }
    }
}
