namespace Server.Security.Util;

public class Pkdf2Utils
{
    private static int totalParts = 8;
    private static int minLengthSalt = 400;

    public static byte[] RandomBytes(int length)
    {
        var bytes = new byte[length];
        var random = new Random();
        random.NextBytes(bytes);
        return bytes;
    }

    public static int GetLength(int length)
    {
        decimal rest = 0;

        do
        {
            length++;
            rest = Convert.ToDecimal(length % 2);
        } while (rest != 0);

        return (length < minLengthSalt) ? GetLength(length) : length;
    }

    public static byte[] GetSaltBytes(int length) => Pkdf2Utils.RandomBytes(GetLength(length));

    public static Pbkdf2ValueInfo GetParts(byte[] value)
    {
        List<byte[]> parts = new List<byte[]>();
        var size = value.Length / totalParts;

        for (int x = 0; x < totalParts; x++)
        {
            List<byte> bytes = new List<byte>();

            for (int y = (x * size); y < (size * (x + 1)); y++)
                bytes.Add(value[y]);

            parts.Add(bytes.ToArray());
        }

        return new Pbkdf2ValueInfo()
        {
            Value = value,
            Parts = parts
        };
    }

    public static byte[] Write(byte[] derived, byte[] salt)
    {
        List<byte> values = new List<byte>();
        var derivedInfo = GetParts(derived);
        var saltInfo = GetParts(salt);

        for (int a = 0; a < totalParts; a++)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(derivedInfo.Parts[a]);
            bytes.AddRange(saltInfo.Parts[a]);
            values.AddRange(bytes.ToArray());
        }

        return values.ToArray();
    }

    public static Pbkdf2DerivationInfo Read(byte[] value, int length)
    {
        var parts = GetParts(value);
        var size = length / totalParts;
        var info = new Pbkdf2DerivationInfo();

        for (var a = 0; a < parts.Parts.Count; a++)
        {
            var part = parts.Parts[a];
            List<byte> derivedBytes = new List<byte>();
            List<byte> saltBytes = new List<byte>();

            for (int i = 0; i < size; i++) derivedBytes.Add(part[i]);
            for (int i = size; i < part.Length; i++) saltBytes.Add(part[i]);

            info.Derivated.AddRange(derivedBytes.ToArray());
            info.Salt.AddRange(saltBytes.ToArray());
        }

        return info;
    }
}