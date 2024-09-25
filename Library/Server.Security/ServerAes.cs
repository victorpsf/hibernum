using System.Security.Cryptography;
using Server.Library;

namespace Server.Security;

public class ServerAes
{
    private readonly string Password;
    private readonly string Salt;

    public ServerAes(string password, string salt)
    {
        this.Password = password;
        this.Salt = salt;
    }

    private Rfc2898DeriveBytes GetKey()
        => new(this.Password, Binary.FromString(this.Salt).Bytes);

    private void AddInfoKey(Aes aesAlg, Rfc2898DeriveBytes key)
    {
        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
        aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
    }
    
    public Binary Encrypt(string plainText)
    {
        byte[] encrypted;
        using Aes aesAlg = Aes.Create();
        this.AddInfoKey(aesAlg, this.GetKey());

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using (var msEncrypt = new MemoryStream())
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using (var swEncrypt = new StreamWriter(csEncrypt))
                swEncrypt.Write(plainText);
            encrypted = msEncrypt.ToArray();            
        }

        return Binary.FromBytes(encrypted);
    }

    public Binary Decrypt(Binary value)
    {
        string plaintext = null;

        using Aes aesAlg = Aes.Create();
        this.AddInfoKey(aesAlg, this.GetKey());
        
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msDecrypt = new MemoryStream(value.Bytes))
        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            plaintext = srDecrypt.ReadToEnd();

        return Binary.FromString(plaintext);
    }

    public static ServerAes Create(string password, string salt)
        => new ServerAes(password, salt);
}