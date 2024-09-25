using Server.Library;
using Server.Security;

namespace Server.Properties;

public class DatabaseProperties
{
    public ServerPropertieManager Manager { get; private set; }
    public ServerAes Aes { get; private set; }

    public DatabaseProperties(ServerPropertieManager manager, ServerAes aes)
    {
        this.Manager = manager;
        this.Aes = aes;
    }

    public string AuthenticationContextConnectionString
    {
        get
        {
#if RELEASE
            return this.Aes.Decrypt(Binary.FromBase64(this.Manager.EnvKey("364a0781299d59c5645f187c0f03274d1c4ec9d7a1d25d6aa52bed18af2485e08e7f1bae176bca88d1ba1869ed38af2fade66c638830f2d14d5488a5574a8753"))).ToBinary();
#elif DEBUG
            return this.Manager.GetSecret("Connections", "Databases", "AuthenticationConnectionString") ?? string.Empty;
#endif
        }
    }
    
    public string HibernumContextConnectionString
    {
        get
        {
#if RELEASE
            return this.Aes.Decrypt(Binary.FromBase64(this.Manager.EnvKey("22ce82c8eed1835967d71ff7d1f352623c98b40dd06e570f658e1f65b4eebe01796225849feb0f2d1db01d62a157757e642a028203ef803dfc5bb4bd89fff616"))).ToBinary();
#elif DEBUG
            return this.Manager.GetSecret("Connections", "Databases", "HibernumConnectionString") ?? string.Empty;
#endif
        }
    }
}