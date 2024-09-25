using Microsoft.EntityFrameworkCore;
using Server.Database.Entity;
using Server.Database.Entity.Relational;
using Server.Database.Sequence;
using Server.Properties;

namespace Server.Database.Contexts;

public class AuthenticationContext: DbContext
{
    public ServerProperties serverProperties { get; private set; }
    
    public DbSet<AuthEntity> Auth { get; set; }
    public DbSet<CompanyEntity> Company { get; set; }
    public DbSet<AuthCompanyEntity> AuthCompany { get; set; }

    public AuthenticationContext(ServerProperties serverProperties, DbContextOptions<AuthenticationContext> contextOptions) : base(contextOptions) {
        this.serverProperties = serverProperties;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(this.serverProperties.DatabaseProperties.AuthenticationContextConnectionString);

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<AuthEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<AuthEntitySequenceGenerator>();
            entity.Property(a1 => a1.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasMany(a1 => a1.AuthCompanyEntities)
                .WithOne(a2 => a2.Auth)
                .HasForeignKey("authid");
        });
        
        builder.Entity<CompanyEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<CompanyEntitySequenceGenerator>();
            entity.Property(a1 => a1.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasMany(a1 => a1.AuthCompanyEntities)
                .WithOne(a2 => a2.Company)
                .HasForeignKey("companyid");
        });
        
        builder.Entity<AuthCompanyEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<AuthCompanyEntitySequenceGenerator>();
            entity.HasOne(a1 => a1.Auth)
                .WithMany(a2 => a2.AuthCompanyEntities)
                .HasForeignKey("authid");
            entity.HasOne(a1 => a1.Company)
                .WithMany(a2 => a2.AuthCompanyEntities)
                .HasForeignKey("companyid");
        });
    }
}
