using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.EntityFrameworkCore.EntityFramework.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, string dbProvider)
        {
            // اینجا provider رو داری و می‌تونی شرط بزاری
            if (dbProvider == "PostgreSql")
            {
                // عملیات Seed برای PostgreSQL
            }
            else
            {
                // عملیات Seed برای SQL Server
            }
        }
    }
}
