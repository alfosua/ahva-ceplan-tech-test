namespace Ahva.Ceplan.Portal;

public class SecurityOptions
{
    public const string SectionName = "Security";

    public int SessionExpirationMinutes { get; set; } = 20;

    public int SessionWarningSeconds { get; set; } = 60;

    public int BanTimeMinutes { get; set; } = 15;

    public int MaxRetries { get; set; } = 5;
}
