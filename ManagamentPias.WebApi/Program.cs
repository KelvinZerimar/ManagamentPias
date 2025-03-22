using ManagementPias.App;
using ManagementPias.Infra.Persistence;
using ManagementPias.Infra.Persistence.Contexts;
using ManagementPias.Infra.Persistence.Options;
using ManagementPias.Infra.Shared;
using ManagementPias.Infra.Shared.Authentication.Settings;
using ManagementPias.WebApi.CORS;
using ManagementPias.WebApi.Extensions;
using ManagementPias.WebApi.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Application startup services registration");
// Add configuration to the DI container
builder.Services.AddSingleton(builder.Configuration);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddApplicationLayer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
    if (databaseOptions is null)
    {
        throw new ArgumentNullException(nameof(databaseOptions));
    }
    options.UseNpgsql(databaseOptions.ConnectionString,
    npgsqlAction =>
    {
        npgsqlAction.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        npgsqlAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
        npgsqlAction.CommandTimeout(databaseOptions.CommandTimeout);
    });
    options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
    options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
});
//
builder.Services.AddPersistenceInfrastructure();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddSwaggerExtension();
builder.Services.AddControllersExtension();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// CORS
builder.Services.AddCorsExtension();
builder.Services.AddHealthChecks();
//API Security
builder.Services.AddJWTAuthentication(builder.Configuration.GetMyOptions<AuthenticationSettings>());
// API version
builder.Services.AddApiVersioningExtension();
// Add authentication
builder.Services.ConfigureServices(builder.Configuration);
// CORS
builder.Services.AddMyCorsConfiguration(builder.Configuration);

var app = builder.Build();
Log.Information("Application startup middleware registration");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    // for quick database (usually for prototype)
    //using (var scope = app.Services.CreateScope())
    //{
    //    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    // use context
    //    dbContext.Database.EnsureCreated();
    //}

    // Aplicar migraciones al iniciar la aplicación
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate(); // Aplica las migraciones pendientes
    }
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Enable CORS
app.UseCors();
app.UseSwaggerExtension();
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");

Log.Information("Application Starting");

app.Run();
