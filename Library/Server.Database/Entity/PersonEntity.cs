using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Entity.Enumerables;

namespace Server.Database.Entity;

[Table(name: "person", Schema = "public")]
public class PersonEntity
{
    [Column(name: "id")]
    public long Id { get; set; }
    
    [Column(name: "type")]
    public PersonType Type { get; set; }

    [Column(name: "name")] 
    public string Name { get; set; } = string.Empty;

    [Column(name: "birth_date")]
    public DateTime BirthDate { get; set; }
    
    [Column(name: "created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column(name: "deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    public long CompanyId { get; set; }
    public Collection<PersonContactEntity> Contacts { get; set; } = new();
    public Collection<PersonAddressEntity> Address { get; set; } = new();
    public Collection<PersonDocumentEntity> Documents { get; set; } = new();
}