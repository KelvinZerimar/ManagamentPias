using ManagamentPias.App.Common.Services;
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

        // Configuración del servicio de encriptación
        var encryptionKey = _config["EncryptionOptions:Key"];
        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentNullException(nameof(encryptionKey), "Encryption key cannot be null or empty.");
        }
        var encryptionService = new EncryptionService(encryptionKey);
        services.AddSingleton<IEncryptionService>(encryptionService);
    }
}
