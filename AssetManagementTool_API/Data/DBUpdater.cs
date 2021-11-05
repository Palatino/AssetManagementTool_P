using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data
{
    public class DbUpdater : IDbUpdater
    {
        private readonly AssetManagementDbContext _context;
        public DbUpdater(AssetManagementDbContext context)
        {
            _context = context;
        }

        //This method will apply any pending migration in the production environment
        public void ApplyPendingMigrations()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
            {
                return;
            }

            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }
        }
    }
}
