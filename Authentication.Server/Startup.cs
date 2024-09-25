using Server.Core;
using Server.Models.Core;

namespace Authentication.Server;

public class Startup: StartupCore
{
    public Startup(IConfiguration configuration): 
        base(configuration, "auth", false, new(){ DatabaseName.AUTHENTICATION }, Startup.ConfigureAnotherServices)
    { }

    public static void ConfigureAnotherServices(StartupCore ctx, IServiceCollection services)
    { }
}