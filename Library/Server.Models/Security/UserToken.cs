namespace Server.Models.Security;

public class UserToken
{
    public string Token { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}