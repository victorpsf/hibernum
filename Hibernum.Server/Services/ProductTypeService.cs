using Hibernum.Server.Services.Queries;
using Hibernum.Server.Services.Relations;
using Hibernum.Server.Services.Rules;
using Server.Database.Connector;
using Server.Database.Models;
using Server.Dims;
using Server.Dtos;
using Server.Exceptions;
using Server.Models.Database;

namespace Hibernum.Server.Services;

public partial class ProductTypeService
{
    private ConnectorFactory factory;

    private ProductService ProductService
    { get => new (this.factory); }

    public ProductTypeService(ConnectorFactory factory)
    { this.factory = factory; }
    
    public List<ProductType> FindProductType(List<Product> products)
    {
        this.Build(
            ProductTypeSql.FindProductTypeByRelationSql,
            products,
            out string sql,
            out ParameterCollection collection
        );

        var results = new List<ProductType>();
        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            results.AddRange(
                client.ExecuteReader<ProductType>(new()
                {
                    Sql = sql,
                    Parameter = collection
                })
            );
        }

        catch
        { }
        
        finally { client.Disconnect(); }

        return results;
    }

    public List<ProductType> Find(FindProductTypeRule rule)
    {
        this.Build(
            ProductTypeSql.FindProductTypeSql,
            rule,
            out string sql,
            out ParameterCollection collection
        );

        var client = this.factory.getHibernumClient();
        var types = new List<ProductType>();

        try
        {
            client.Connect();
            types.AddRange(
                client.ExecuteReader<ProductType>(new ()
                {
                    Sql = sql,
                    Parameter = collection
                })
            );
        }

        catch
        { }

        finally
        { client.Disconnect(); }

        return types;
    }

    public void LoadRelation(
        ProductTypeRelation relation,
        List<ProductType> productTypes
    )
    {
        if (relation.Product)
        {
            var results = this.ProductService.Find(new FindProductRule()
            {
                Products = productTypes.Select(a => a.ProductId)
                    .ToList()
            });

            foreach (var productType in productTypes)
                productType.Product = results.Where(a => a.Id == productType.ProductId)
                    .FirstOrDefault();            
        }
    }

    public void Remove(List<ProductType> productTypes)
    { }

    private ProductType Insert(ProductType productType)
    {
        var client = this.factory.getHibernumClient();

        try
        {
            client.Connect();
            var id = client.Execute(new BancoExecuteScalarArgument()
            {
                Sql = ProductTypeSql.InsertProductTypeSql,
                Sequence = ProductTypeSql.SequenceName,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@value", productType.Value)
                    .Add("@productid", productType.ProductId)
                    .Add("@created_at", productType.CreatedAt)
            });
            productType.Id = id;
            client.Commit();
        }

        catch (Exception ex)
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        { client.Disconnect(); }

        return productType;
    }
    
    private ProductType Update(ProductType productType)
    {
        var client = this.factory.getHibernumClient();
        try

        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductTypeSql.UpdateProductTypeSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@value", productType.Value)
                    .Add("@productid", productType.ProductId)
                    .Add("@id", productType.Id)
            });
            client.Commit();
        }

        catch
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        {
            client.Disconnect();
        }

        return productType;
    }

    public ProductType Save(ProductTypeDim dim)
    {
        var products = this.ProductService.Find(new FindProductRule()
        {
            Id = dim.Product
        });

        if (!products.Any())
            throw new Exception();

        if (dim.Id > 0)
        {
            var productType = this.Find(new FindProductTypeRule()
            {
                Id = dim.Id
            }).FirstOrDefault();

            if (productType is null)
                throw new Exception();

            return this.Update(new ProductType()
            {
                Id = productType.Id,
                ProductId = dim.Product ?? 0,
                Product = products.FirstOrDefault(),
                Value = dim.Value
            });
        }

        return this.Insert(new ProductType()
        {
            Value = dim.Value,
            ProductId = dim.Product ?? 0,
            Product = products.FirstOrDefault(),
            CreatedAt = DateTime.UtcNow
        });
    }

    public ProductType Remove(long id)
    {
        var result = this.Find(new FindProductTypeRule() { Id = id }).FirstOrDefault();

        if (result is null)
            throw new BusinessException(BusinessExceptionEnum.PRODUCT_TYPE_NOT_EXISTS);
        
        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductTypeSql.RemoveProductTypeSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@id", result.Id)
                    .Add("@date", DateTime.UtcNow)
            });
            client.Commit();
        }

        catch
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        { client.Disconnect(); }

        return result;
    }
}