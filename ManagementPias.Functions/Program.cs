using ManagementPias.App;
using ManagementPias.App.Common.Services;
using ManagementPias.App.Interfaces;
using ManagementPias.App.Interfaces.Repositories;
using ManagementPias.Functions.Extensions;
using ManagementPias.Functions.Middlewares;
using ManagementPias.Functions.Options;
using ManagementPias.Infra.Persistence.Contexts;
using ManagementPias.Infra.Persistence.Repositories;
using ManagementPias.Infra.Shared.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Configuration = ManagementPias.Infra.Persistence.Configuration;

var host = new HostBuilder()
   .ConfigureOpenApi()
   .ConfigureFunctionsWebApplication(builder =>
   {
       builder.UseMiddleware<ExceptionHandlingMiddleware>();
   })
   .ConfigureLogging(logging =>
   {
       logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
       logging.AddFilter((category, level) => level >= LogLevel.Information);
       logging.AddConsole();
   })
   //.ConfigureAppConfiguration(c =>
   //{
   //    c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
   //        .AddEnvironmentVariables();
   //})
   .ConfigureServices((context, services) =>
   {
       var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
       services.AddSingleton(Configuration.AppSettings);
       services.ConfigureOptions<DatabaseOptionsSetup>();
       services.AddApplicationLayer();

       services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
       {
           options.UseNpgsql(connectionString);
       });

       //Infra.Persistence  
       services.AddSingleton(serviceProvider =>
       {
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
           return new CosmosClient(Configuration.AppSettings["Values:CosmosDb:Account"], Configuration.AppSettings["Values:CosmosDb:Key"], options);
       });

       services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
       services.AddTransient<IPostgresUserRepositoryAsync, PostgresUserRepositoryAsync>();
       services.AddScoped<INoteRepository>(s =>
       {
           var cosmosClient = s.GetRequiredService<CosmosClient>();
           return new CosmosNoteRepository(
               cosmosClient,
               Configuration.AppSettings["Values:CosmosDb:DatabaseName"]!,
               Configuration.AppSettings["Values:CosmosDb:ContainerNotes"]!
           );
       });
       services.AddSingleton<ICosmosUserRepository>(s =>
       {
           var cosmosClient = s.GetRequiredService<CosmosClient>();
           return new CosmosUserRepositoryAsync(
               cosmosClient,
               Configuration.AppSettings["Values:CosmosDb:DatabaseName"]!,
               Configuration.AppSettings["Values:CosmosDb:ContainerUsers"]!
           );
       });
       services.AddTransient<IAssetRepositoryAsync, AssetRepositoryAsync>();
       //Infra.Shared  
       services.AddTransient<IDateTimeService, DateTimeService>();
       var encryptionKey = Configuration.AppSettings["Values:EncryptionOptions:Key"];
       var encryptionService = new EncryptionService(encryptionKey!);
       services.AddSingleton<IEncryptionService>(encryptionService);
       //App.Common  
       services.ConfigureServices(Configuration.AppSettings);
       //  
       //services.AddHttpClient(nameof(BitcoinPriceChecker));

   })
   .Build();

host.Run();
