using ManagamentPias.App.Interfaces;
using ManagamentPias.Infra.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManagamentPias.Infra.Shared;

public static class ServiceRegistration
{
    public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();
    }
}
