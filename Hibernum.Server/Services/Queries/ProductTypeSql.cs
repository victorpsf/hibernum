namespace Hibernum.Server.Services.Queries;

public class ProductTypeSql
{
    public static string SequenceName = @"public.product_type_sequence_generator";
    
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
    and pt.deleted_at is null
";

    public static string UpdateProductTypeSql = @"
update public.producttype
    set value = @value,
        productid = @productid
where id = @id
";

    public static string InsertProductTypeSql = @"
insert into public.producttype
    (id, value, productid, created_at)
values 
    (@id, @value, @productid, @created_at)
";
    
    public static string RemoveProductTypeSql = @"
update public.producttype
    set deleted_at = @date
where id = @id
";
}