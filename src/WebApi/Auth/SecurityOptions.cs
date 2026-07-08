namespace Ahva.Ceplan.WebApi.Auth;

public class SecurityOptions
{
    public const string SectionName = "Security";

    public int BanTimeMinutes { get; set; } = 15;

    public int MaxRetries { get; set; } = 5;
}
