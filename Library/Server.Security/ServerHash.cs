using System.Security.Cryptography;
using Server.Library;
using Server.Models.Security;

namespace Server.Security;

public class ServerHash
{
    private readonly ServerHashAlgorithm cipher = ServerHashAlgorithm.SHA512;

    private ServerHash(ServerHashAlgorithm cipher)
    { this.cipher = cipher; }

    private byte[] Encipher(HashAlgorithm alg, byte[] value) => alg.ComputeHash(value);

    private HashAlgorithm _SHA512() => SHA512.Create();
    private HashAlgorithm _SHA384() => SHA384.Create();
    private HashAlgorithm _SHA256() => SHA256.Create();

    private byte[] Update(byte[] value)
    {
        HashAlgorithm alg;

        switch (this.cipher)
        {
            case ServerHashAlgorithm.SHA512: alg = this._SHA512(); break;
            case ServerHashAlgorithm.SHA384: alg = this._SHA384(); break;
            case ServerHashAlgorithm.SHA256: alg = this._SHA256(); break;
            default:
                throw new ArgumentException("CIPHER IS NOT SUPPORTED");
        };

        return this.Encipher(alg, value);
    }

    public Binary Update(string value)
        => Binary.FromBytes(this.Update(Binary.FromString(value).Bytes));

    public static ServerHash Create(ServerHashAlgorithm cipher) 
        => new(cipher);
}