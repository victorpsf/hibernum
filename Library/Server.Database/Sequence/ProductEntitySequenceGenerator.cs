using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Server.Database.Sequence;

public class ProductEntitySequenceGenerator: ValueGenerator<long>
{
    public override long Next(EntityEntry entry)
        => entry.Context.Database.SqlQueryRaw<long>("select nextval('public.product_sequence_generator') as id").ToList().FirstOrDefault();

    public override bool GeneratesTemporaryValues => false;
}