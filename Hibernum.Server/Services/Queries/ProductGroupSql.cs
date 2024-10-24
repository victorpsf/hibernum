namespace Hibernum.Server.Services.Queries;

public class ProductGroupSql
{
    public static string SequenceName = "public.product_group_sequence_generator";
    
    public static string FindProductGroup = @"
select 
    distinct
        pg.id           as id,
        pg.name         as name,
        pg.created_at   as created_at,
        pg.deleted_at   as deleted_at
from public.productgroup as pg
";
    
    public static string FindProductGroupByProductRelationSql = @$"
{FindProductGroup}
inner join public.product as p on p.groupid = pg.id
where pg.deleted_at is null 
";

    public static string FindProductByRelationSql = @"
select 
    distinct
        p.id as id,
        p.name as name,
        p.size as size,
        p.groupid as groupid,
        p.created_at as created_at,
        p.deleted_at as deleted_at
from public.product as p
where p.deleted_at is null
    and p.groupid = any(:ids)
";

    public static string UpdateProductGroupSql = @"
update public.productgroup
    set name = @name
where id = @id
";
    
    public static string InsertProductGroupSql = @"
insert into public.productgroup
    (id, name, created_at)
values 
    (@id, @name, @createdat)
";
    
    public static string RemoveProductGroupSql = @"
update public.productgroup
    set deleted_at = @date
where id = @id
";
}