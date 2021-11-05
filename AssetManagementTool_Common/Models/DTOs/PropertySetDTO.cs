using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{
    public class PropertySetDTO
    {
        public string name { get; set; }
        public PropertyDTO[] properties { get; set; }
    }

}
