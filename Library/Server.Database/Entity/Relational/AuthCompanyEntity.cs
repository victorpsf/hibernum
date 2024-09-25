using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Server.Database.Entity.Relational;

[Table(name: "authxcompany", Schema = "public")]
public class AuthCompanyEntity
{
    [Column(name: "id")]
    public long Id { get; set; }

    public AuthEntity Auth { get; set; } = new();
    public CompanyEntity Company { get; set; } = new();
}