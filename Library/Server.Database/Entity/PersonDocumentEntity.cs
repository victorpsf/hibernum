using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Entity.Enumerables;

namespace Server.Database.Entity;

[Table(name: "personaddress", Schema = "public")]
public class PersonDocumentEntity
{
    [Column(name: "id")]
    public long Id { get; set; }
    
    [Column(name: "type")]
    public PersonDocumentType Type { get; set; }

    [Column(name: "value")] 
    public string Value { get; set; } = string.Empty;

    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public PersonEntity Person { get; set; } = new();
}