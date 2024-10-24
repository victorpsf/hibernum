using Hibernum.Server.Services.Rules;
using Server.Database.Models;
using Server.Extensions;
using Server.Models.Database;

namespace Hibernum.Server.Services;

public partial class FileService
{
    private void Build(
        string baseSql,
        List<Product> products,
        out string sql,
        out ParameterCollection collection
    )
    {
        List<string> closures = new();
        collection = new();

        if (!products.Any())
            throw new Exception();
        
        closures.Add("p.id = any(@id)");
        collection.Add("@id", products.Select(a => a.Id).ToArray());

        sql = closures.Any() ? $"{baseSql} AND {string.Join(" AND ", closures.ToArray())}" : baseSql;
    }
    
    private void Build(
        string baseSql,
        FindFileRule rule,
        out string sql,
        out ParameterCollection collection
    )
    {
        List<string> closures = new();
        collection = new();

        if (rule.Id is not null && rule.Id > 0)
        {
            closures.Add("f.id = @id");
            collection.Add("@id", rule.Id);
        }

        if (!string.IsNullOrEmpty(rule.Name))
        {
            closures.Add("f.name like @name");
            collection.Add("@name", rule.Name.ToLike().ToUpper());
        }
        
        
        if (!string.IsNullOrEmpty(rule.Mime))
        {
            closures.Add("f.mine like @mime");
            collection.Add("@mime", rule.Mime.ToLike().ToUpper());
        }

        sql = closures.Any() ? $"{baseSql} AND {string.Join(" AND ", closures.ToArray())}" : baseSql;
    }
}