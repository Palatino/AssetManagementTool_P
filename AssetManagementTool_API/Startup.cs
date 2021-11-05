using AssetManagementTool_API.Data;
using AssetManagementTool_API.Data.Repositories;
using AssetManagementTool_API.Data.Repositories.IRepositories;
using AssetManagementTool_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AssetManagementTool_API.Services.IServices;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AssetManagementTool_API.Authorization;

namespace AssetManagementTool_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Authority"];
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });


            services.AddDbContext<AssetManagementDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AssetManagementTool_API", Version = "v1" });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.AssetCreate, policy =>
                    policy.Requirements.Add(new HasPermissionRequirement(Policy.AssetCreate, Configuration["Auth0:Authority"])));

                options.AddPolicy(Policy.AssetDelete, policy =>
                    policy.Requirements.Add(new HasPermissionRequirement(Policy.AssetDelete, Configuration["Auth0:Authority"])));

                options.AddPolicy(Policy.AttachmentCreate, policy =>
                    policy.Requirements.Add(new HasPermissionRequirement(Policy.AttachmentCreate, Configuration["Auth0:Authority"])));

                options.AddPolicy(Policy.AttachmentDelete, policy =>
                    policy.Requirements.Add(new HasPermissionRequirement(Policy.AttachmentDelete, Configuration["Auth0:Authority"])));
            });


            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IDbUpdater, DbUpdater>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped(IServiceProvider => new BlobServiceClient(Configuration["BLOB_CONNECTION_STRING"]));
            services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbUpdater dbUpdater)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetManagementTool_API v1"));
            }
            dbUpdater.ApplyPendingMigrations();
            app.UseHttpsRedirection();

            //Cors should be configure properly
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
