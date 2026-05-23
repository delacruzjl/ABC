namespace ABC.Management.Api.Settings;

public class ExternalAuthSettings
{
    public const string SectionName = "ExternalAuth";

    public GoogleSettings Google { get; set; } = new();
    public AzureEntraSettings AzureEntra { get; set; } = new();
}

public class GoogleSettings
{
    public string ClientId { get; set; } = string.Empty;
}

public class AzureEntraSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
}
