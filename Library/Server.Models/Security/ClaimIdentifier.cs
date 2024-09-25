using System.Security.Claims;

namespace Server.Models.Security;

public class ClaimIdentifier
{
    public long Id { get; set; }
    public long Company { get; set; }

    public static ClaimIdentifier GetInstance(IEnumerable<Claim> claims)
    {
        var aI = claims.Where(a => a.Type == "aI").Single();
        var cI = claims.Where(a => a.Type == "cI").Single();

        return new ClaimIdentifier()
        {
            Id = Convert.ToInt64(aI.Value),
            Company = Convert.ToInt64(cI.Value)
        };
    }
}