using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Models.DTOs
{

    public class NewAssetDTO
    {
        [Required(ErrorMessage ="Provide asset name")]
        public string Name { get; set; }
        [Required]
        [Range(-90,90, ErrorMessage ="Latitude value must be between -90 and 90 degress")]
        public double Latitude { get; set; }
        [Required]
        [Range(-180,180,ErrorMessage ="Longitud value must be between -90 and 90 degress")]
        public double Longitude { get; set; }

        public string AddedBy { get; set; }
        public byte[] IFCFile { get; set; }

    }
}
