using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Extensions
{
    public static class ByteArrayExtension
    {
        //Check if byte[] is an image file if return format, return empty string if not valid format
        public static string GetImageExtension(this byte[] bytes)
        {

            string[] formats = new string[] { "bmp", "gif", "gif", "png", "tiff", "tiff", "jpg" };

            var bmp = new byte[] { 0x42, 0x4D };               // BMP "BM"
            var gif87a = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };     // "GIF87a"
            var gif89a = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };     // "GIF89a"
            var png = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };   // PNG "\x89PNG\x0D\0xA\0x1A\0x0A"
            var tiffI = new byte[] { 0x49, 0x49, 0x2A, 0x00 }; // TIFF II "II\x2A\x00"
            var tiffM = new byte[] { 0x4D, 0x4D, 0x00, 0x2A }; // TIFF MM "MM\x00\x2A"
            var jpeg = new byte[] { 0xFF, 0xD8, 0xFF };        // JPEG JFIF (SOI "\xFF\xD8" and half next marker xFF)

            byte[][] headers = new byte[][] { bmp, gif87a, gif89a, png, tiffI, tiffM, jpeg };

            for (int i = 0; i < headers.Count(); i++)
            {
                byte[] header = headers[i];
                for (int u = 0; u < header.Count(); u++)
                {
                    if (header[u] != bytes[u])
                    {
                        break;
                    }

                    if (u == header.Count() - 1)
                    {
                        return formats[i];
                    }
                }

            }

            return "";


        }
    }
}
