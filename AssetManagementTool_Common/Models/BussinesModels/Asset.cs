using System;
using System.Collections.Generic;

namespace AssetManagementTool_Common.Models.BussinesModels
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public string AddedBy { get; set; }
        public string IFCBlobName { get; set; }
        public IEnumerable<CommentAttachment> Comments { get; set; } = new List<CommentAttachment>();
        public IEnumerable<ImageAttachment> Images { get; set; } = new List<ImageAttachment>();
        public IEnumerable<FileAttachment> Files { get; set; } = new List<FileAttachment>();

    }
}
