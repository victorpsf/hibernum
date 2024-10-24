using Hibernum.Server.Services.Queries;
using Hibernum.Server.Services.Relations;
using Hibernum.Server.Services.Rules;
using Server.Database.Connector;
using Server.Database.Connector.Client;
using Server.Database.Models;
using Server.Dims;
using Server.Exceptions;
using Server.Models.Database;

namespace Hibernum.Server.Services;

public partial class ProductDescriptionService
{
    private ConnectorFactory factory;

    private ProductService ProductService
    { get => new ProductService(this.factory); }

    public ProductDescriptionService(ConnectorFactory factory)
    {
        this.factory = factory;
    }
    
    public List<ProductDescription> FindProductDescription(List<Product> product)
    {
        this.Build(
            ProductDescriptionSql.FindProductDescriptionByRelationSql,
            product,
            out string sql,
            out ParameterCollection collection
        );

        var results = new List<ProductDescription>();
        var client = this.factory.getHibernumClient();

        try
        {
            client.Connect();
            results.AddRange(
                client.ExecuteReader<ProductDescription>(new()
                {
                    Sql = sql,
                    Parameter = collection
                })
            );
        }

        catch {}
        
        finally { client.Disconnect(); }

        return results;
    }

    public void LoadRelation(
        ProductDescriptionRelation relation,
        List<ProductDescription> descriptions
    ) {
        if (relation.Product)
        {
            var results = this.ProductService.Find(new FindProductRule() { Products = descriptions.Select(a => a.ProductId).ToList() });

            foreach (var description in descriptions)
                description.Product = results.Where(a => a.Id == description.ProductId).FirstOrDefault();
        }
    }

    public List<ProductDescription> Find(FindProductDescriptionRule rule)
    {
        this.Build(
            ProductDescriptionSql.FindProductDescriptionSql,
            rule,
            out string sql,
            out ParameterCollection parameters
        );
        
        var client = this.factory.getHibernumClient();
        var descriptions = new List<ProductDescription>();

        try
        {
            client.Connect();
            descriptions.AddRange(
                client.ExecuteReader<ProductDescription>(new ()
                {
                    Sql = sql,
                    Parameter = parameters
                })
            );
        }

        catch
        { }

        finally
        { client.Disconnect(); }

        return descriptions;
    }
    
    private ProductDescription Update(ProductDescription productDescription)
    {
        var client = this.factory.getHibernumClient();

        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductDescriptionSql.UpdateProductDescriptionSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@value", productDescription.Value)
                    .Add("@productid", productDescription.ProductId)
                    .Add("@id", productDescription.Id)
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

        return productDescription;
    }
    
    private ProductDescription Insert(ProductDescription productDescription)
    {
        var client = this.factory.getHibernumClient();

        try
        {
            client.Connect();
            var id = client.Execute(new BancoExecuteScalarArgument()
            {
                Sql = ProductDescriptionSql.InsertProductDescriptionSql,
                Sequence = ProductDescriptionSql.SequenceName,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@value", productDescription.Value)
                    .Add("@productid", productDescription.ProductId)
                    .Add("@created_at", productDescription.CreatedAt)
            });
            productDescription.Id = id;
            client.Commit();
        }

        catch (Exception ex)
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        { client.Disconnect(); }

        return productDescription;
    }

    public ProductDescription Save(ProductDescriptionDim dim)
    {
        var products = this.ProductService.Find(new FindProductRule()
        {
            Id = dim.Product
        });

        if (!products.Any())
            throw new Exception();
        
        if (dim.Id > 0)
        {
            var productDescription = this.Find(new FindProductDescriptionRule()
            {
                Id = dim.Id
            }).FirstOrDefault();

            if (productDescription is null)
                throw new Exception();

            return this.Update(new ProductDescription()
            {
                Id = productDescription.Id,
                ProductId = dim.Product ?? 0,
                Product = products.FirstOrDefault(),
                Value = dim.Value
            });
        }

        return this.Insert(new ProductDescription()
        {
            Value = dim.Value,
            ProductId = dim.Product ?? 0,
            Product = products.FirstOrDefault(),
            CreatedAt = DateTime.UtcNow
        });
    }
    
    public ProductDescription Remove(long id)
    {
        var result = this.Find(new FindProductDescriptionRule() { Id = id }).FirstOrDefault();

        if (result is null)
            throw new BusinessException(BusinessExceptionEnum.PRODUCT_DESCRIPTION_NOT_EXISTS);
        
        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductDescriptionSql.RemoveProductDescriptionSql,
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