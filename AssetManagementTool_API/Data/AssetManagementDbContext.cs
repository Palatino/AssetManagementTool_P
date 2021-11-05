using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagementTool_Common.Models.BussinesModels;

namespace AssetManagementTool_API.Data
{
    public class AssetManagementDbContext : DbContext
    {
        public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options)
            :base (options)
        {}

        public DbSet<Asset> Assets { get; set; }
        public DbSet<CommentAttachment> AssetsComments { get; set; }
        public DbSet<FileAttachment> AssetsFiles { get; set; }
        public DbSet<ImageAttachment> AssetsImages { get; set; }

    }
}
