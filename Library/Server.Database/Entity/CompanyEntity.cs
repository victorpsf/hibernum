using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Entity.Relational;

namespace Server.Database.Entity;

[Table(name: "company", Schema = "public")]
public class CompanyEntity
{
    [Column(name: "id")]
    public long Id { get; set; }

    [Column(name: "name")] 
    public string Name { get; set; } = string.Empty;
    
    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public Collection<AuthCompanyEntity> AuthCompanyEntities { get; set; } = new();
}