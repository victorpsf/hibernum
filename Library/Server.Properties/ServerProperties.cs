using Server.Library;

namespace Server.Properties;

public class ServerProperties
{
    public ServerPropertieManager Manager { get; private set; }
    public SecurityProperties SecurityProperties { get; private set; }
    public DatabaseProperties DatabaseProperties { get; private set; }
    
    public ServerProperties(ServerPropertieManager manager)
    {
        this.Manager = manager;

        this.SecurityProperties = new SecurityProperties(manager);
        this.DatabaseProperties = new DatabaseProperties(manager, this.SecurityProperties.Eas);
    }
}