namespace Server.Security.Util;

public class Pbkdf2DerivationInfo
{
    public List<byte> Derivated { get; set; } = new();
    public List<byte> Salt { get; set; } = new();
}