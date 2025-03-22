using ManagementPias.App.Common.Services;
using ManagementPias.Infra.Shared.Authentication.Settings;
using ManagementPias.Infra.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManagementPias.Functions.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Core
        services.AddScoped<ITokenService, JwtTokenService>();

        // add singleton AuthenticationSettings to DI with values the values
        //var values = configuration.GetSection("Values:AuthenticationSettings");
        //var valuesAuthenticationSettings = values.Get<AuthenticationSettings>();
        //services.AddSingleton<AuthenticationSettings>(valuesAuthenticationSettings!);

        services.AddSingleton<AuthenticationSettings>(provider => configuration.GetSection("Values:AuthenticationSettings")!.Get<AuthenticationSettings>()!);
    }
}
