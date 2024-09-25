using Serilog;

namespace Server.Core;

public static class ProgramCore
{
    public static void ConfigureLog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();
    }
}