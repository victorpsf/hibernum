using Microsoft.EntityFrameworkCore;
using Server.Database.Entity;
using Server.Database.Entity.Relational;
using Server.Database.Sequence;
using Server.Properties;

namespace Server.Database.Contexts;

public class HibernumContext: DbContext
{
    public ServerProperties serverProperties { get; private set; }
    
    public DbSet<AuthEntity> Auth { get; set; }
    public DbSet<CompanyEntity> Company { get; set; }
    public DbSet<AuthCompanyEntity> AuthCompany { get; set; }
    public DbSet<PersonEntity> Person { get; set; }
    public DbSet<PersonAddressEntity> Address { get; set; }
    public DbSet<PersonContactEntity> Contact { get; set; }
    public DbSet<PersonDocumentEntity> Document { get; set; }
    public DbSet<ProductGroupEntity> ProductGroup { get; set; }
    public DbSet<ProductEntity> Product { get; set; }
    public DbSet<ProductTypeEntity> ProductType { get; set; }
    public DbSet<ProductDescriptionEntity> ProductDescription { get; set; }

    public HibernumContext(ServerProperties serverProperties, DbContextOptions<HibernumContext> contextOptions) : base(contextOptions) {
        this.serverProperties = serverProperties;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(this.serverProperties.DatabaseProperties.HibernumContextConnectionString);
    
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
        
        builder.Entity<PersonEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<PersonEntitySequenceGenerator>();
            entity.Property(a1 => a1.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasMany(a1 => a1.Contacts)
                .WithOne(a2 => a2.Person)
                .HasForeignKey("personid");
            entity.HasMany(a1 => a1.Address)
                .WithOne(a2 => a2.Person)
                .HasForeignKey("personid");
            entity.HasMany(a1 => a1.Documents)
                .WithOne(a2 => a2.Person)
                .HasForeignKey("personid");
        });
        
        builder.Entity<PersonContactEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<PersonEntitySequenceGenerator>();
            entity.Property(a1 => a1.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasOne(a1 => a1.Person)
                .WithMany(a2 => a2.Contacts)
                .HasForeignKey("personid");
        });
        
        builder.Entity<PersonAddressEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<PersonAddressEntitySequenceGenerator>();
            entity.Property(a1 => a1.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasOne(a1 => a1.Person)
                .WithMany(a2 => a2.Address)
                .HasForeignKey("personid");
        });
        
        builder.Entity<PersonDocumentEntity>(entity =>
        {
            entity.Property(a1 => a1.Id)
                .HasValueGenerator<PersonDocumentEntitySequenceGenerator>();
            entity.Property(a1 => a1.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasOne(a1 => a1.Person)
                .WithMany(a2 => a2.Documents)
                .HasForeignKey("personid");
        });

        builder.Entity<ProductGroupEntity>(entity =>
        {
            entity.Property(a => a.Id)
                .HasValueGenerator<ProductGroupEntitySequenceGenerator>();
            entity.Property(a => a.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasMany(a => a.Products)
                .WithOne(b => b.Group)
                .HasForeignKey("groupid");
        });
        builder.Entity<ProductEntity>(entity =>
        {
            entity.Property(a => a.Id)
                .HasValueGenerator<ProductEntitySequenceGenerator>();
            entity.Property(a => a.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasOne(a => a.Group)
                .WithMany(b => b.Products)
                .HasForeignKey("groupid");
        });
        builder.Entity<ProductTypeEntity>(entity =>
        {
            entity.Property(a => a.Id)
                .HasValueGenerator<ProductTypeEntitySequenceGenerator>();
            entity.Property(a => a.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasOne(a => a.Product)
                .WithMany(b => b.Types)
                .HasForeignKey("productid");
        });
        builder.Entity<ProductDescriptionEntity>(entity =>
        {
            entity.Property(a => a.Id)
                .HasValueGenerator<ProductDescriptionEntitySequenceGenerator>();
            entity.Property(a => a.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);
            entity.HasOne(a => a.Product)
                .WithMany(b => b.Descriptions)
                .HasForeignKey("productid");
        });
    }
}