using Hibernum.Server.Services.Rules;
using Server.Database.Models;
using Server.Extensions;
using Server.Models.Database;

namespace Hibernum.Server.Services;

public partial class ProductGroupService
{
    public void Build(
        string baseSql,
        List<Product> products,
        out string sql,
        out ParameterCollection collection
    ) {
        collection = new();
        
        if (products.Any())
        {
            sql = $"{baseSql} and p.id = any(:ids)";
            collection.Add(":ids", products.Select(a => a.Id).ToArray());
        }

        else throw new Exception();
    }

    public void Build(
        string baseSql,
        FindProductGroupRule rule,
        out string sql,
        out ParameterCollection collection
    ) {
        collection = new();
        List<string> parts = new();

        if (rule.Id is not null)
        {
            collection.Add(":id", rule.Id);
            parts.Add("pg.id = :id");
        }

        if (!string.IsNullOrEmpty(rule.Name))
        {
            collection.Add(":name", rule.Name.ToLike().ToUpper());
            parts.Add("upper(pg.name) like :name");
        }

        sql = parts.Any() ? $"{baseSql} where pg.deleted_at is null and {string.Join(" and ", parts)}": $"{baseSql} where pg.deleted_at is null";
    }
}