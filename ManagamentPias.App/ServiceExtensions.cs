using ManagamentPias.App.Helpers;
using ManagamentPias.App.Interfaces;
using ManagamentPias.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ManagamentPias.App;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {

        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        // Register MediatR pipeline behavior
        services.AddScoped<IDataShapeHelper<Asset>, DataShapeHelper<Asset>>();
        services.AddScoped<IDataShapeHelper<Portfolio>, DataShapeHelper<Portfolio>>();
        services.AddScoped<IDataShapeHelper<Rating>, DataShapeHelper<Rating>>();
        services.AddScoped<IModelHelper, ModelHelper>();
    }
}
