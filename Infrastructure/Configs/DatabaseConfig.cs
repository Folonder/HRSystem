namespace Infrastructure.Configs;

public class DatabaseConfig
{
    public const string Section = "DatabaseConfig";

    public string ConnectionString { get; set; } = string.Empty;
}