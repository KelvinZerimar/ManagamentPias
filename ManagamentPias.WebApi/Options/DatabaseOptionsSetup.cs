using ManagamentPias.Infra.Persistence.Options;
using Microsoft.Extensions.Options;

namespace ManagamentPias.WebApi.Options;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        options.ConnectionString = connectionString ?? throw new Exception();

        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
