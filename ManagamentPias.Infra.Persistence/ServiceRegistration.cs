using ManagementPias.App.Interfaces;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.Infra.Persistence.Options;
using ManagementPias.Infra.Persistence.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ManagementPias.Infra.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services)
        {

            //services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            //{
            //    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
            //    options.UseSqlServer(databaseOptions.ConnectionString,
            //        sqlServerAction =>
            //        {
            //            sqlServerAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            //            sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount); // The Better Way to Configure Entity Framework Core - Milan Jovanovic
            //            sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout); // The Better Way to Configure Entity Framework Core - Milan Jovanovic
            //        });
            //    options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            //    options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            //});

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
            services.AddTransient<IPostgresUserRepositoryAsync, PostgresUserRepositoryAsync>();
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
            services.AddSingleton<ICosmosUserRepository>(s =>
            {
                var cosmosClient = s.GetRequiredService<CosmosClient>();
                var databaseOptions = s.GetService<IOptions<DatabaseOptions>>()!.Value;
                return new CosmosUserRepositoryAsync(
                    cosmosClient,
                    databaseOptions.DatabaseName,
                    databaseOptions.ContainerUsers
                );
            });
            #endregion Repositories
        }
    }
}

