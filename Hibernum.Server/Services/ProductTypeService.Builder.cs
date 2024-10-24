using System.Data;
using Server.Database.Models;
using Server.Models.Database;
using Hibernum.Server.Services.Rules;

namespace Hibernum.Server.Services;

public partial class ProductTypeService
{
    public void Build(
        string baseSql,
        List<Product> products,
        out string sql,
        out ParameterCollection collection
    )
    {
        collection = new ParameterCollection();

        if (products.Any())
            collection.Add(
                "@id",
                products.Select(a => a.Id).ToArray(), 
                ParameterDirection.Input
            );
        else
            throw new Exception();

        sql = baseSql;
    }

    public void Build(
        string baseSql,
        FindProductTypeRule rule,
        out string sql,
        out ParameterCollection collection
    )
    {
        List<string> closures = new();
        collection = new();

        if (rule.Id is not null && rule.Id > 0)
        {
            closures.Add("pt.id = @id");
            collection.Add("@id", rule.Id);
        }

        if (rule.Product is not null && rule.Product > 0)
        {
            closures.Add("pt.productid = @productid");
            collection.Add("@productid", rule.Product);
        }

        if (!string.IsNullOrEmpty(rule.Value))
        {
            closures.Add("upper(pt.value) like @value");
            collection.Add("@value", rule.Value);
        }

        closures.Add("pt.deleted_at is null");
        sql = closures.Any() ? $"{baseSql} where {string.Join(" and ", closures.ToArray())}" : baseSql;
    }
}