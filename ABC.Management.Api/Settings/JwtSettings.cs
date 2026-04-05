namespace ABC.Management.Api.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string[] Audiences { get; set; } = [];
    public int ExpirationMinutes { get; set; } = 480;
    public string Key { get; set; } = string.Empty;
}
