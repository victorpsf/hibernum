using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Entity.Relational;

namespace Server.Database.Entity;

[Table(name: "auth", Schema = "public")]
public class AuthEntity
{
    [Column(name: "id")]
    public long Id { get; set; }

    [Column(name: "name")] 
    public string Name { get; set; } = string.Empty;
    
    [Column(name: "email")]
    public string Email { get; set; } = string.Empty;
    
    [Column(name: "passphrase")]
    public string Passphrase { get; set; } = string.Empty;
    
    [Column(name: "enabled")]
    public bool Enabled { get; set; }

    [Column(name: "force_passphrase_change")]
    public bool ForcePasssphraseChange { get; set; }
    
    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    public Collection<AuthCompanyEntity> AuthCompanyEntities { get; set; } = new();
}