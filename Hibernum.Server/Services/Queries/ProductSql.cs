namespace Hibernum.Server.Services.Queries;

public class ProductSql
{
    public static string SequenceName = "public.product_sequence_generator";
    
    public static string FindProductSql = @"
select 
    p.id as id,
    p.name as name,
    p.size as size,
    p.fileid as fileid,
    p.groupid as groupid,
    p.created_at as created_at,
    p.deleted_at as deleted_at
from public.product as p
where p.deleted_at is null
";

    public static string UpdateProductSql = @"
update public.product 
	set name = @name,
		size = @size,
		groupid = @group,
		fileid = @fileid
where id = @id
";
    
    public static string DeleteProductSql = @"
update public.product 
	set deleted_at = @deletedAt
where id = @id
";

    public static string InsertProductSql = @"
insert into public.product
    (id, groupid, name, size, created_at, fileid)
values 
    (@id, @groupid, @name, @size, @createdat, @fileid)
";
}