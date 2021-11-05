using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Data
{
    public interface IDbUpdater
    {
        public void ApplyPendingMigrations();
    }
}
