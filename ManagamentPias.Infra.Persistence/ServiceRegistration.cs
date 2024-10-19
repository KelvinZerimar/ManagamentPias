using ManagamentPias.Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManagamentPias.Infra.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            #region Repositories

            //services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            //services.AddTransient<IPositionRepositoryAsync, PositionRepositoryAsync>();
            //services.AddTransient<IEmployeeRepositoryAsync, EmployeeRepositoryAsync>();
            //services.AddTransient<ICustomerRepositoryAsync, CustomerRepositoryAsync>();

            #endregion Repositories
        }
    }
}
