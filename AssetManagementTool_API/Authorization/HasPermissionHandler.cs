using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Authorization
{
    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {

            // If user does not have the scope permission, get out of here
            if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var permissionClaims = context.User.Claims.Where(c => c.Type == "permissions" && c.Issuer == requirement.Issuer);
            foreach (var permClaim in permissionClaims)
            {
                var scopes = permClaim.Value.Split(' ');
                if (scopes.Any(s => s == requirement.Permission))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

            }
            return Task.CompletedTask;
        }
    }
}
