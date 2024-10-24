using Server.Models.Database;
using Hibernum.Server.Services.Rules;
using Server.Extensions;


namespace Hibernum.Server.Services;

public partial class ProductService
{
    public void Build(
        string baseSql,
        FindProductRule rule,
        out string sql,
        out ParameterCollection collection
    )
    {
        List<string> closures = new();
        collection = new();
        
        if (rule.Id is not null && rule.Id > 0)
        {
            closures.Add("p.id = @id");
            collection.Add("@id", rule.Id);
        }
        
        else if (rule.Products is not null && rule.Products.Any())
        {
            closures.Add("p.id = any(@id)");
            collection.Add("@id", rule.Products.ToArray());
        }

        if (!string.IsNullOrEmpty(rule.Name))
        {
            closures.Add("upper(p.name) like @name");
            collection.Add("@name", rule.Name.ToLike().ToUpper());
        }

        if (!string.IsNullOrEmpty(rule.Size))
        {
            closures.Add("p.size = @size");
            collection.Add("@size", rule.Size);
        }

        sql = closures.Any() ? $"{baseSql} AND {string.Join(" AND ", closures)}" : baseSql;
    }
}