namespace Server.Library;

public class Binary
{
    private byte[] bytes;

    private Binary(byte[] bytes)
    { this.bytes = bytes; }

    public byte[] Bytes { get { return this.bytes; } }

    public string ToBinary()
        => string.Join("", this.bytes.Select(a => Convert.ToChar(a)).Select(a => a.ToString()).ToArray());

    public string ToBase64()
        => Convert.ToBase64String(this.bytes);

    public string ToHex()
        => Convert.ToHexString(this.bytes);

    public static Binary FromBase64(string value)
        => new Binary(Convert.FromBase64String(value));

    public static Binary FromHex(string value)
        => new Binary(Convert.FromHexString(value));

    public static Binary FromString(string value)
        => new Binary(value.ToCharArray().Select(a => Convert.ToByte(a)).ToArray());
    
    public static Binary FromBytes(byte[] bytes)
        => new Binary(bytes);

    public static Binary concat(byte[] v1, byte[] v2)
    {
        List<byte> bytes = new List<byte>();
        
        foreach (byte b1 in v1) bytes.Add(b1);
        foreach (byte b2 in v2) bytes.Add(b2);

        return Binary.FromBytes(bytes.ToArray());
    }
}