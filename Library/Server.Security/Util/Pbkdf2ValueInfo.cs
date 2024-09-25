namespace Server.Security.Util;

public class Pbkdf2ValueInfo
{
    public byte[] Value { get; set; } = new byte[] { };
    public List<byte[]> Parts { get; set; } = new();
}