namespace Server.Services.Database.Query;

public class ProductTypeSql
{
    public static string FindProductTypeSql = @"
select 
    pt.id           as id,
    pt.value        as value,
    pt.productid    as productid,
    pt.created_at   as created_at,
    pt.deleted_at   as deleted_at
from public.producttype as pt
";
    
    public static string FindProductTypeByRelationSql = @$"
{FindProductTypeSql}
inner join public.product as p on p.id = pt.productid
where p.id = any(@id)
";
}