using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Authorization
{
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }
        public string Issuer { get; }

        public HasPermissionRequirement(string permission, string issuer)
        {
            Permission = permission;
            Issuer = issuer;
        }
    }
}
