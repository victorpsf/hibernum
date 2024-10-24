namespace Server.Services.Database.Query;

public static class ProductGroupSql
{
    public static string FindProductGroupSql = @"
select 
    pg.id           as id,
    pg.name         as name,
    pg.created_at   as created_at,
    pg.deleted_at   as deleted_at
from public.productgroup as pg
";
}