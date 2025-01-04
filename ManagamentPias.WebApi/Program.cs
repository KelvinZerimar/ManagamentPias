using ManagamentPias.App;
using ManagamentPias.Infra.Persistence;
using ManagamentPias.Infra.Persistence.Contexts;
using ManagamentPias.Infra.Shared;
using ManagamentPias.WebApi.Extensions;
using ManagamentPias.WebApi.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Application startup services registration");
// Add configuration to the DI container
builder.Services.AddSingleton(builder.Configuration);

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddApplicationLayer();
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
// API version
builder.Services.AddApiVersioningExtension();

var app = builder.Build();

Log.Information("Application startup middleware registration");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    // for quick database (usually for prototype)
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // use context
        dbContext.Database.EnsureCreated();
    }
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Enable CORS
app.UseCors("AllowAll");
app.UseSwaggerExtension();
app.UseErrorHandlingMiddleware();
app.UseHealthChecks("/health");

Log.Information("Application Starting");

app.Run();
