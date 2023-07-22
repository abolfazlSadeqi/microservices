namespace APIGateway.Config;

public class CustomRateLimitOptions
{
    public const string CustomRateLimit = "CustomRateLimit";
    public int PermitLimit { get; set; } = 1;
    public int Minute { get; set; } = 1;
    public bool AutoReplenishment { get; set; } = true;
    public int HttpStatusCode { get; set; } = 429;
    public int QueueLimit { get; set; } = 1;


}
