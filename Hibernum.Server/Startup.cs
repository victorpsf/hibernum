using Server.Core;
using Server.Models.Core;

namespace Hibernum.Server;

public class Startup: StartupCore
{
    public Startup(IConfiguration configuration): 
        base(configuration, "v1", false, new(){ DatabaseName.AUTHENTICATION, DatabaseName.HIBERNUM }, Startup.ConfigureAnotherServices)
    { }

    public static void ConfigureAnotherServices(StartupCore ctx, IServiceCollection services)
    { }
}