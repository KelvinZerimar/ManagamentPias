using ManagementPias.App.Helpers;
using ManagementPias.App.Interfaces;
using ManagementPias.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ManagementPias.App;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        // Register MediatR pipeline behavior
        services.AddScoped<IDataShapeHelper<Asset>, DataShapeHelper<Asset>>();
        services.AddScoped<IDataShapeHelper<Rating>, DataShapeHelper<Rating>>();
        services.AddScoped<IModelHelper, ModelHelper>();
    }
}
