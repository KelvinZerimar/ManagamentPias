using ManagementPias.Infra.Persistence.Options;
using Microsoft.Extensions.Options;

namespace ManagementPias.WebApi.Options;

public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string DefaultConnection = "DefaultConnection";
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly string CosmosDb = "CosmosDb";
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString(DefaultConnection);

        options.ConnectionString = connectionString ?? throw new Exception();

        _configuration.GetSection(ConfigurationSectionName).Bind(options);
        _configuration.GetSection(CosmosDb).Bind(options);
    }
}
