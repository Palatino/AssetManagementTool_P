using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Services;
using AssetManagementTool_Client.Services.IServices;
using AssetManagementTool_Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using AssetManagementTool_Client.Authorization;

namespace AssetManagementTool_Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
                options.UserOptions.ScopeClaim = "openid email profile";
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            builder.Services.AddHttpClient("AuthenticatedClient",
            client => client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")))
           .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
           .ConfigureHandler(
            authorizedUrls: new[] { builder.Configuration.GetValue<string>("BaseAPIUrl") }
            ));

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
              .CreateClient("AuthenticatedClient"));

            builder.Services.AddMudServices();
            builder.Services.AddScoped<IAssetService, AssetService>();
            builder.Services.AddScoped<IAssetAttachmentService, AssetAtachmentService>();

            await builder.Build().RunAsync();
        }
    }
}
