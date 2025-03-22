using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ManagementPias.App;

public static class Injector
{
    public static void RegisterApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddValidatorsFromAssembly(typeof(Assembly).Assembly);
        services.AddMediatR(cfg => cfg.
            RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        //services.AddScoped<IPasswordHashService, PasswordHashService>();
    }
}