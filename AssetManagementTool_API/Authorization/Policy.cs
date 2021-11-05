using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Authorization
{
    /// <summary>
    /// Class containing the names of the different policies
    /// </summary>
    public static class Policy
    {
        public const string AssetCreate = "asset:create";
        public const string AssetDelete = "asset:delete";
        public const string AttachmentCreate = "attachment:create";
        public const string AttachmentDelete = "attachment:delete";
    }
}
