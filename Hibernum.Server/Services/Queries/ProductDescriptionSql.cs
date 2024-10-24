namespace Hibernum.Server.Services.Queries;

public class ProductDescriptionSql
{
    public static string SequenceName = "public.product_description_sequence_generator";
    
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
    and pd.deleted_at is null
";
    
    public static string UpdateProductDescriptionSql = @"
update public.productdescription
    set value = @value,
        productid = @productid
where id = @id
";

    public static string InsertProductDescriptionSql = @"
insert into public.productdescription
    (id, value, productid, created_at)
values 
    (@id, @value, @productid, @created_at)
";
    
    public static string RemoveProductDescriptionSql = @"
update public.productdescription
    set deleted_at = @date
where id = @id
";
}