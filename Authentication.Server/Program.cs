using Serilog;
using Server.Core;

namespace Authentication.Server;

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