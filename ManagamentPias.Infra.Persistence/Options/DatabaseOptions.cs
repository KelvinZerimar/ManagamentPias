namespace ManagamentPias.Infra.Persistence.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }

    public string Account { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ContainerNotes { get; set; } = string.Empty;
    public string ContainerUsers { get; set; } = string.Empty;
}
