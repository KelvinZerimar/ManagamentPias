using ManagamentPias.App.Interfaces;
using ManagamentPias.App.Interfaces.Repositories;
using ManagamentPias.Infra.Persistence.Contexts;
using ManagamentPias.Infra.Persistence.Options;
using ManagamentPias.Infra.Persistence.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ManagamentPias.Infra.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services)
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

            services.AddSingleton(serviceProvider =>
            {
                var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
                var options = new CosmosClientOptions
                {
                    // Otras configuraciones opcionales...
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                        IgnoreNullValues = true
                    }
                };

                AppContext.SetSwitch("AzureCosmosDisableNewtonSoftJsonCheck", true);
                return new CosmosClient(databaseOptions.Account, databaseOptions.Key, options);
            });


            #region Repositories

            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IAssetRepositoryAsync, AssetRepositoryAsync>();
            services.AddScoped<INoteRepository>(s =>
            {
                var cosmosClient = s.GetRequiredService<CosmosClient>();
                var databaseOptions = s.GetService<IOptions<DatabaseOptions>>()!.Value;
                return new CosmosNoteRepository(
                    cosmosClient,
                    databaseOptions.DatabaseName,
                    databaseOptions.ContainerNotes
                );
            });
            services.AddSingleton<IUserRepository>(s =>
            {
                var cosmosClient = s.GetRequiredService<CosmosClient>();
                var databaseOptions = s.GetService<IOptions<DatabaseOptions>>()!.Value;
                return new UserRepositoryAsync(
                    cosmosClient,
                    databaseOptions.DatabaseName,
                    databaseOptions.ContainerUsers
                );
            });
            #endregion Repositories
        }
    }
}

