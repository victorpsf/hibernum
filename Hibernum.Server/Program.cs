using Serilog;
using Server.Core;
using Server.Models.Security;
using Server.Security;

namespace Hibernum.Server;

public class Program
{
    public static void Main(string[] arguments)
    {
        ProgramCore.ConfigureLog();
        
        try
        {
            Program.CreateHost(arguments)
                .Build()
                .Run();
        }
        
        catch (Exception ex)
        { Log.Fatal(ex.Message); }
    }

    public static IHostBuilder CreateHost(string[] arguments)
        => Host.CreateDefaultBuilder(arguments)
            .UseSerilog()
            .ConfigureWebHostDefaults(endpoint =>
            {
                endpoint.UseStartup<Startup>();
            });
}