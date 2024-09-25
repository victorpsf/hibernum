using Server.Database.Contexts;

namespace Server.Services.Database;

public class HibernumDbService
{
    private readonly HibernumContext ctx;

    public HibernumDbService(HibernumContext ctx)
        => this.ctx = ctx;
}