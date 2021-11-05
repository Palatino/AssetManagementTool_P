using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models
{
    public class BlobInfo
    {

        public Stream content { get; set; }
        public string contentType { get; set; }

        public BlobInfo(Stream content, string contentType)
        {
            this.content = content;
            this.contentType = contentType;
        }
    }
}
