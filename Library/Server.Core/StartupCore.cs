using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Server.Database.Contexts;
using Server.Library;
using Server.Middleware;
using Server.Models.Core;
using Server.Properties;
using Server.Services;
using Server.Services.Database;

namespace Server.Core;

public delegate void ConfigureAnotherServices(StartupCore ctx, IServiceCollection services);

public class StartupCore
{
    public ServerPropertieManager PropertieManager { get; private set; }
    public string Prefix { get; private set; }
    public bool EnableCors { get; private set; } = false;
    public List<DatabaseName> Databases { get; private set; } = new();
    private ConfigureAnotherServices? ConfigureAnotherServices;

    private string RoutePattern
    { get => string.Concat($"/{this.Prefix.ToLower()}" ?? "/api", "/{controller}/{action=Index}"); }

    public StartupCore(
        IConfiguration configuration,
        string prefix,
        bool enableCors,
        List<DatabaseName> databases,
        ConfigureAnotherServices? configure
    ) {
        this.PropertieManager = new(configuration);
        this.Prefix = prefix;
        this.EnableCors = enableCors;
        this.Databases.AddRange(databases);
        this.ConfigureAnotherServices = configure;
    }

    public StartupCore(
        IConfiguration configuration,
        string prefix,
        bool enableCors,
        List<DatabaseName> databases
    ): this(configuration, prefix, enableCors, databases, null) 
    { }

    public void ConfigureDatabases(IServiceCollection services)
    {
        foreach (DatabaseName name in this.Databases)
            switch (name)
            {
                case DatabaseName.AUTHENTICATION:
                    services.AddDbContext<AuthenticationContext>();
                    services.AddScoped<AuthenticationDbService>();
                    break;
                case DatabaseName.HIBERNUM:
                    services.AddDbContext<HibernumContext>();
                    services.AddScoped<HibernumDbService>();
                    break;
            }
    }
    
    public void ConfigureSecurity(IServiceCollection services)
    {
        services.AddScoped<JwtSecurity>();

        var properties = new ServerProperties(this.PropertieManager);
        services.AddAuthentication(
            x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer(
            JwtBearerDefaults.AuthenticationScheme,
            x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(properties.SecurityProperties.TokenSecret),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = properties.SecurityProperties.TokenIssuer
                };
            }
        );
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<ServerPropertieManager>(provider => this.PropertieManager);
        services.AddScoped<ServerProperties>();
        
        this.ConfigureDatabases(services);
        this.ConfigureSecurity(services);
        
        services.AddScoped<LoggedUser>();

        if (this.ConfigureAnotherServices is not null)
            this.ConfigureAnotherServices(this, services);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandler>();
        
        if (!this.EnableCors)
            app.UseCors(a =>
            {
                a.AllowAnyHeader();
                a.AllowAnyMethod();
                a.AllowAnyOrigin();
            });

        Log.Information(this.RoutePattern);
        app.UseEndpoints(enpoint =>
        {
            enpoint.MapControllerRoute(
                this.Prefix,
                this.RoutePattern
            );
        });
    }
}
