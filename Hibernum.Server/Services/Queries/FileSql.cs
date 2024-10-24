namespace Hibernum.Server.Services.Queries;

public class FileSql
{
    public static string SequenceName = "public.file_sequence_generator";

    public static string SelectFileSql = @"
select 
    f.id                as id,
    f.name              as name,
    f.mime              as mime,
    f.size              as size,
    f.last_modified     as lastModified,
    f.path              as path
from public.file as f 
where f.deleted_at is null
";
    
    public static string FindFileByProductRelationSql = @$"
select 
    f.id                as id,
    f.name              as name,
    f.mime              as mime,
    f.size              as size,
    f.last_modified     as lastModified,
    f.path              as path
from public.file as f 
inner join public.product as p on p.fileid = f.id
where f.deleted_at is null
    and p.deleted_at is null 
";

    public static string UpdateFileSql = @"
update public.file
    set name = @name,
        mime = @mime,
        size = @size,
        last_modified = @last_modified,
        path = @path
where id = @id
";

    public static string InsertFileSql = @"
insert into public.file 
    (id, name, mime, size, last_modified, path, created_at)
values
    (@id, @name, @mime, @size, @last_modified, @path, @CreatedAt)
";
    
    public static string RemoveFileSql = @"
update public.file
    set deleted_at = @date
where id = @id
";
}