using Microsoft.Extensions.Configuration;

namespace Server.Library;

public class ServerPropertieManager
{
    public IConfiguration Configuration { get; private set; }
    private string Separator
    {
        get
        {
#if RELEASE
            return "";
#elif DEBUG
            return ":";
#endif
        }
    }

    private string PrepareSection(string section)
    {
#if RELEASE
            return section.ToUpper();
#elif DEBUG
        return $"{section.Substring(0, 1).ToUpper()}{section.Substring(1).ToLower()}";
#endif

    }

    public ServerPropertieManager(IConfiguration configuration)
    { this.Configuration = configuration; }
    
    private string ToJsonPath(params string[] parameters)
        => string.Join(
            this.Separator,
            parameters.Select(a => this.PrepareSection(a)).ToArray()
        );
    
    public string? EnvKey(params string[] parameters)
        => Environment.GetEnvironmentVariable(string.Join("", parameters));
    
    public string? GetSecret(params string[] parameters)
        => this.Configuration.GetSection(this.ToJsonPath(parameters)).Value;
}
