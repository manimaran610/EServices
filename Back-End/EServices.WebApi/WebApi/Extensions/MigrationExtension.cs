
using System;
using Infrastructure.Identity.Contexts;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.Extensions
{
    public static class MigrationExtension
    {

        public static void ApplyEFCoreMigration(this IHost host, IConfiguration configuration)
        {
            bool canApplyMigration = configuration.GetValue<bool>("ApplyMigration");
            if (canApplyMigration)
            {
                try
                {
                    using (var scope = host.Services.CreateScope())
                    {
                        Serilog.Log.Information("Started Applying Migration - ApplicationDbContext");
                        using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                        {
                            context.Database.Migrate();
                        }
                        Serilog.Log.Information("Done Migration ApplicationDbContext");

                        Serilog.Log.Information("Started Applying Migration - IdentityContext");
                        using (var context = scope.ServiceProvider.GetService<IdentityContext>())
                        {
                            context.Database.Migrate();
                        }
                        Serilog.Log.Information("Done Migration IdentityContext");
                    }
                }
                catch (Exception ex)
                {
                    Serilog.Log.Information($"Applying Migration -{ex.Message}");
                }
            }
        }
    }
}