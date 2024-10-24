namespace Server.Database.Connector.Client;

public class HibernumDbClient: PostgresClient
{
    public HibernumDbClient(string connectionString): base(connectionString)
    { }
}