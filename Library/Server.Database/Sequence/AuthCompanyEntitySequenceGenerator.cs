using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Server.Database.Sequence;

public class AuthCompanyEntitySequenceGenerator: ValueGenerator<long>
{
    public override long Next(EntityEntry entry)
        => entry.Context.Database.SqlQueryRaw<long>("select nextval('public.auth_company_sequence_generator') as id").ToList().FirstOrDefault();

    public override bool GeneratesTemporaryValues => false;
}