using ManagamentPias.App.Interfaces;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.Infra.Persistence.Contexts;
using ManagamentPias.Infra.Persistence.Options;
using ManagamentPias.Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ManagamentPias.Infra.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
                options.UseSqlServer(databaseOptions.ConnectionString,
                    sqlServerAction =>
                    {
                        sqlServerAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount); // The Better Way to Configure Entity Framework Core - Milan Jovanovic
                        sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout); // The Better Way to Configure Entity Framework Core - Milan Jovanovic
                    });
                options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });
            #region Repositories

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IAssetRepositoryAsync, AssetRepositoryAsync>();

            #endregion Repositories
        }
    }
}

