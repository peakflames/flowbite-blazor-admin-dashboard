namespace WebApp.Domain;

public class SessionInfo
{
    public string Device { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public string ActionHref { get; set; } = "#";
    public string ActionButtonText { get; set; } = "Revoke";
    public Type? Icon { get; set; }
    public string? IconSize { get; set; }
    public string? IconClass { get; set; }
}
