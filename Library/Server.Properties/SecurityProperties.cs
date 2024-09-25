using Server.Library;
using Server.Security;

namespace Server.Properties;

public class SecurityProperties
{
    public ServerPropertieManager Manager { get; private set; }
    private static ServerAes? eas;
    
    public SecurityProperties(ServerPropertieManager manager)
        => this.Manager = manager;

    public ServerAes Eas
    {
        get
        {
            if (eas is null)
                eas = ServerAes.Create(this.Password, this.Salt);
            return eas;
        }
    }
    
    public string Password
    {
        get
        {
#if RELEASE
            return this.Manager.EnvKey("10d9679dfd14f527ac84db2d433f9e26cd28eb52a4000d32fb47b4868599dfb6fed245ecb476544b28afe7eb1cd1da16cfa87d355dccf585a07226a26ee60bc6") ?? string.Empty;
#elif DEBUG
            return this.Manager.GetSecret("Security", "Cryptography", "Password") ?? string.Empty;
#endif
        }
    }
    
    public string Salt
    {
        get
        {
#if RELEASE
            return this.Manager.EnvKey("4bf133677673bb0ba8e76fa01d17b65873ce45f9939b334f2816e181e11c8e7bb102b75d06fc601450582dfc222709cd44a0a442c7278dc62a509e7abe198bd3") ?? string.Empty;
#elif DEBUG
            return this.Manager.GetSecret("Security", "Cryptography", "Salt") ?? string.Empty;
#endif
        }
    }

    public string TokenIssuer
    {
        get
        {
#if RELEASE
            return this.Eas.Decrypt(Binary.FromBase64(this.Manager.EnvKey("86933e33b4d013341aecff44b61d8cb4df85d1518bc9f90577979bc14896097e757a88d2dfd02f15ccfa77a314f80f9f389c5ab99b9df7f749b66447cedeb054") ?? string.Empty)).ToBinary();
#elif DEBUG
            return this.Manager.GetSecret("Security", "Jwt", "Issuer") ?? string.Empty;
#endif
        }
    }
    
    public string TokenType
    {
        get
        {
#if RELEASE
            return this.Eas.Decrypt(Binary.FromBase64(this.Manager.EnvKey("1b7304afdd0f3b655e222c9cccba9e254984059178193e20d0c76cb05ed14d932912b52dc4990db3bf2a9872030db20b94fd820a12a81f24cc61b32dc0ab01d7") ?? string.Empty)).ToBinary();
#elif DEBUG
            return this.Manager.GetSecret("Security", "Jwt", "Type") ?? string.Empty;
#endif
        }
    }
    
    public byte[] TokenSecret
    {
        get
        {
#if RELEASE
            return this.Eas.Decrypt(Binary.FromBase64(this.Manager.EnvKey("45eabbaeb06ec41e516320bf7d96832aae878904ec95b9077422e4ba36d3e6f7b077a6265c5beb363f6630056b6f54822bea0e17667629ef458bdb548ed553cc") ?? string.Empty)).Bytes;
#elif DEBUG
            return Binary.FromString(this.Manager.GetSecret("Security", "Jwt", "Secret") ?? string.Empty).Bytes;
#endif
        }
    }
    
    public int TokenMinutes
    {
        get
        {
#if RELEASE
            var value = this.Eas.Decrypt(Binary.FromBase64(this.Manager.EnvKey("25f5ecf53084d93f8836ea137cf5d2194ec998f948a8938cbee8e1e6f1b465cb8e605cde2b2ffb092ecee33dc4d3bfab34efdc967d7a3a9dd91d8ab1933b2666") ?? string.Empty)).ToString();
            return Convert.ToInt32(value);
#elif DEBUG
            return Convert.ToInt32(this.Manager.GetSecret("Security", "Jwt", "Minutes") ?? "240");
#endif
        }
    }
}