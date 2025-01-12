using ManagementPias.WebApi.CORS.Settings;
using ManagementPias.WebApi.Options;
using System.Diagnostics.CodeAnalysis;

namespace ManagementPias.WebApi.CORS;

[ExcludeFromCodeCoverage]
internal static class CorsStartup
{
    public static void AddMyCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetMyOptions<CorsSettings>();

        if (corsSettings == null)
            return;

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(
                    corsSettings.AllowedOrigins)
                .Build();
            });
        });
    }
}

