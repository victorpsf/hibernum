namespace Server.Services.Database.Query;

public class ProductDescriptionSql
{
    public static string FindProductDescriptionSql = @"
select
    pd.id           as id,
    pd.value        as value,
    pd.productid    as productid,
    pd.created_at   as created_at,
    pd.deleted_at   as deleted_at
from public.productdescription as pd
";

    public static string FindProductDescriptionByRelationSql = @$"
{FindProductDescriptionSql}
inner join product as p on p.id = pd.productid
where p.id = any(@id)
";
}