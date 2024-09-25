using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Server.Library;
using Server.Models.Security;
using Server.Security.Util;

namespace Server.Security;

public class ServerPbkdf2
{
    private static int numBytes = 4096;

    public ServerPbkdf2()
    { }

    public static ServerPbkdf2 GetInstance() => new();

    private KeyDerivationPrf Convert(Pbkdf2HashDerivation hashDerivation) => hashDerivation switch
    {
        Pbkdf2HashDerivation.HMACSHA512 => KeyDerivationPrf.HMACSHA512,
        Pbkdf2HashDerivation.HMACSHA256 => KeyDerivationPrf.HMACSHA256,
        Pbkdf2HashDerivation.HMACSHA1 => KeyDerivationPrf.HMACSHA1,
        _ => throw new ArgumentException(
            $"[ERROR HashDerivation] Pbkdf2Security: {hashDerivation.ToString()} is not defined")
    };

    private int GetInteractionCount(Pbkdf2HashDerivation hashDerivation) => hashDerivation switch {
        Pbkdf2HashDerivation.HMACSHA512 => 210000,
        Pbkdf2HashDerivation.HMACSHA256 => 600000,
        Pbkdf2HashDerivation.HMACSHA1 => 1300000,
        _ => 210000
    };

    private byte[] DeriveValue(string value, byte[] salt, Pbkdf2HashDerivation hashDerivation) =>
        KeyDerivation.Pbkdf2(password: value, salt: salt, prf: Convert(hashDerivation), iterationCount: GetInteractionCount(hashDerivation), numBytesRequested: numBytes);

    public string Write(string value, Pbkdf2HashDerivation hashDerivation)
    {
        var salt = Pkdf2Utils.GetSaltBytes(value.Length);
        var result = this.DeriveValue(value, salt, hashDerivation);

        return Binary.FromBytes(Pkdf2Utils.Write(result, salt)).ToBase64();
    }

    public bool Verify(string derived, string value, Pbkdf2HashDerivation hashDerivation)
    {

        var info = Pkdf2Utils.Read(Binary.FromBase64(derived).Bytes, numBytes);
        var result = this.DeriveValue(value, info.Salt.ToArray(), hashDerivation);

        return Binary.FromBytes(info.Derivated.ToArray()).ToBase64() == Binary.FromBytes(result).ToBase64();
    }
}