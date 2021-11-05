using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementTool_Common.Extensions
{

    public static class FileExtensions
    {
        private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            string contentType;

            bool found = Provider.TryGetContentType(fileName, out contentType);

            if (!found)
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
