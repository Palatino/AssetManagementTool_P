using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.BussinesModels
{
    public abstract class ElementAttachment
    {
        public int Id { get; set; }
        public Asset Asset { get; set; }
        public int AssetId { get; set; }
        public string ElementOwner { get; set; } = null;
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string AddedBy { get; set; }
    }
}
