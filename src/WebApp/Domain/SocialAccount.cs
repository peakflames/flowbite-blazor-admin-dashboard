namespace WebApp.Domain;

public class SocialAccount
{
    public string Platform { get; set; } = "";
    public string Username { get; set; } = "";
    public bool IsConnected { get; set; }
    public Type? Icon { get; set; }
}
