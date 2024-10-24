using System.Data;
using Hibernum.Server.Services.Rules;
using Server.Database.Models;
using Server.Models.Database;

namespace Hibernum.Server.Services;

public partial class ProductDescriptionService
{
    private void Build(
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
                products.Select(a => a.Id).ToArray()
            );
        else
            throw new Exception();

        sql = baseSql;
    }

    private void Build(
        string baseSql,
        FindProductDescriptionRule rule,
        out string sql,
        out ParameterCollection collection
    ) {
        List<string> closures = new();
        collection = new();
        
        if (rule.Id is not null && rule.Id > 0)
        {
            closures.Add("pd.id = @id");
            collection.Add("@id", rule.Id);
        }
        
        if (rule.Product is not null && rule.Product > 0)
        {
            closures.Add("pd.productid = @productid");
            collection.Add("@productid", rule.Product);
        }
        
        if (!string.IsNullOrEmpty(rule.Value))
        {
            closures.Add("upper(pd.value) like @value");
            collection.Add("@value", rule.Value);
        }
        
        closures.Add("pd.deleted_at is null");
        sql = closures.Any() ? $"{baseSql} where {string.Join(" and ", closures.ToArray())}" : baseSql;
    }
}