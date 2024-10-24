using System.Data;
using Hibernum.Server.Services.Relations;
using Server.Database.Connector;
using Server.Database.Connector.Client;
using Server.Database.Models;
using Server.Models.Database;

using Hibernum.Server.Services.Queries;
using Hibernum.Server.Services.Rules;
using NpgsqlTypes;
using Server.Dims;
using Server.Exceptions;
using File = Server.Database.Models.File;

namespace Hibernum.Server.Services;

public partial class ProductService
{
    private ConnectorFactory factory;

    public ProductService(
        ConnectorFactory factory
    ) {
        this.factory = factory;
    }
    
    private ProductGroupService ProductGroup
    { get => new(this.factory); }
    private ProductTypeService ProductType
    { get => new(this.factory); }
    private ProductDescriptionService ProductDescription 
    { get => new(this.factory); }
    private FileService File 
    { get => new(this.factory); }

    public void LoadRelation(
        ProductRelation relation,
        List<Product> products
    ) {
        if (relation.ProductType)
        {
            var types = ProductType.FindProductType(products);

            foreach (var product in products)
                product.Types = types.Where(a => a.ProductId == product.Id).ToList();
        }

        if (relation.ProductDescription)
        {
            var types = ProductDescription.FindProductDescription(products);

            foreach (var product in products)
                product.Descriptions = types.Where(a => a.ProductId == product.Id).ToList();
        }

        if (relation.ProductGroup)
        {
            var results = ProductGroup.FindProductGroups(products);

            foreach (var product in products)
                product.Group = results.Where(a => product.GroupId is not null && a.Id == product.GroupId)
                    .FirstOrDefault();
        }

        if (relation.File)
        {
            var results = this.File.FindProducts(products);

            foreach (var product in products)
                if (product.FileId is not null)
                    product.File = results.Where(a => a.Id == product.FileId)
                        .FirstOrDefault();
        }
    }

    public List<Product> Find(FindProductRule rule)
    {
        this.Build(
            ProductSql.FindProductSql,
            rule,
            out string sql, 
            out ParameterCollection collection
        );

        var results = new List<Product>();

        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            results.AddRange(
                client.ExecuteReader<Product>(new()
                {
                    Sql = sql,
                    Parameter = collection
                })
            );
        }

        catch { }
        
        finally
        { client.Disconnect(); }

        return results;
    }

    private Product Update(Product product)
    {
        var client = this.factory.getHibernumClient();
        try

        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductSql.UpdateProductSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@name", product.Name)
                    .Add("@size", product.Size)
                    .Add("@group", product.GroupId)
                    .Add("@fileid", product.FileId)
                    .Add("@id", product.Id)
            });
            client.Commit();
        }

        catch (Exception e)
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        {
            client.Disconnect();
        }

        return product;
    }

    private Product Insert(Product product)
    {
        var client = this.factory.getHibernumClient();

        try
        {
            client.Connect();
            var id = client.Execute(new BancoExecuteScalarArgument()
            {
                Sql = ProductSql.InsertProductSql,
                Sequence = ProductSql.SequenceName,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@groupid", product.GroupId)
                    .Add("@name", product.Name)
                    .Add("@size", product.Size)
                    .Add("@fileid", product.FileId)
                    .Add("@createdat", DateTime.UtcNow)
            });
            product.Id = id;
            client.Commit();
        }

        catch (Exception ex)
        {
            client.Rollback();
            throw new BusinessException(BusinessExceptionEnum.OPERATION_NOT_SUPPORTED);
        }

        finally
        {
            client.Disconnect();
        }

        return product;
    }

    
    public Product Save(ProductDim dim)
    {
        ProductGroup? productGroup = null;
        File? file = null;

        if (dim.Group is not null)
        {
            var results = this.ProductGroup.Find(new() { Id = dim.Group });

            if (!results.Any())
                throw new BusinessException(BusinessExceptionEnum.PRODUCT_GROUP_NOT_EXISTS);
            
            productGroup = results.FirstOrDefault();
        }

        if (dim.File is not null)
        {
            var results = this.File.Find(new() { Id = dim.File });

            if (!results.Any())
                throw new BusinessException(BusinessExceptionEnum.FILE_NOT_EXISTS);
            
            file = results.FirstOrDefault();
        }

        if (dim.Id > 0)
        {
            var results = this.Find(new() { Id = dim.Id });

            if (!results.Any() || results.Count() > 1)
                throw new BusinessException(BusinessExceptionEnum.NOT_POSSIBLE_SAVE_THIS_PRODUCT);
            
            this.LoadRelation(
                new ()
                {
                    ProductGroup = true
                }, 
                results
            );

            return this.Update(new()
            {
                Id = dim.Id ?? default,
                Name = dim.Name ?? results.FirstOrDefault()?.Name ?? string.Empty,
                Size = dim.Size ?? results.FirstOrDefault()?.Size ?? string.Empty,
                GroupId = dim.Group,
                Group = productGroup,
                FileId = dim.File,
                File = file
            });
        }

        return this.Insert(new Product()
        {
            Name = dim.Name ?? string.Empty,
            Size = dim.Size ?? string.Empty,
            GroupId = productGroup is not null ? productGroup.Id : null,
            Group = productGroup,
            FileId = dim.File,
            File = file
        });
    }

    public Product Remove(long id)
    {
        var product = this.Find(new() { Id = id }).FirstOrDefault();
        
        if (product is null)
            throw new BusinessException(BusinessExceptionEnum.PRODUCT_GROUP_NOT_EXISTS);


        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductSql.DeleteProductSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@id", product.Id)
                    .Add("@deletedAt", DateTime.UtcNow)
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

        return product;
    }
}