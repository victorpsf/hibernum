using Server.Database.Connector.Client;
using Server.Properties;

namespace Server.Database.Connector;

public class ConnectorFactory
{
    public ServerProperties ServerProperties { get; private set; }

    public ConnectorFactory(ServerProperties serverProperties)
    { this.ServerProperties = serverProperties; }

    public HibernumDbClient getHibernumClient()
    { return new (this.ServerProperties.DatabaseProperties.HibernumContextConnectionString); }
}