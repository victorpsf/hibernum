using Hibernum.Server.Services.Queries;
using Hibernum.Server.Services.Relations;
using Hibernum.Server.Services.Rules;
using Server.Database.Connector;
using Server.Database.Models;
using Server.Dims;
using Server.Exceptions;
using Server.Models.Database;

namespace Hibernum.Server.Services;

public partial class ProductGroupService
{
    private ConnectorFactory factory;

    public ProductGroupService(ConnectorFactory factory)
    { this.factory = factory; }
    
    public ProductService Product
    { get => new ProductService(this.factory); }

    public void LoadRelation(
        ProductGroupRelation relation,
        List<ProductGroup> productGroups
    )
    {
        if (relation.Product)
            try
            {
                var results = this.Product.Find(new()
                {
                    Groups = productGroups.Select(a => a.Id).ToList()
                });

                foreach (var productGroup in productGroups)
                    productGroup.Products = results.Where(a => a.GroupId is not null && a.GroupId == productGroup.Id)
                        .ToList();
            }
            catch {}
    }

    public List<ProductGroup> FindProductGroups(List<Product> products)
    {
        var results = new List<ProductGroup>();

        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            this.Build(
                ProductGroupSql.FindProductGroupByProductRelationSql,
                products,
                out string sql,
                out ParameterCollection parameterCollection
            );
            
            results.AddRange(
                client.ExecuteReader<ProductGroup>(new BancoArgument()
                {
                    Sql = sql,
                    Parameter = parameterCollection
                })
            );
        }
        
        catch { }
        
        finally { client.Disconnect(); }

        return results;
    }

    public List<ProductGroup> Find(FindProductGroupRule rule)
    {
        this.Build(
            ProductGroupSql.FindProductGroup,
            rule,
            out string sql,
            out ParameterCollection collection
        );

        List<ProductGroup> results = new();
        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            results.AddRange(
                client.ExecuteReader<ProductGroup>(new ()
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

    public ProductGroup Update(ProductGroup model)
    {
        var client = this.factory.getHibernumClient();
        try

        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductGroupSql.UpdateProductGroupSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@name", model.Name)
                    .Add("@id", model.Id)
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

        return model;
    }
    
    public ProductGroup Insert(ProductGroup model)
    {
        var client = this.factory.getHibernumClient();

        try
        {
            client.Connect();
            var id = client.Execute(new BancoExecuteScalarArgument()
            {
                Sql = ProductGroupSql.InsertProductGroupSql,
                Sequence = ProductGroupSql.SequenceName,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@name", model.Name)
                    .Add("@createdat", DateTime.UtcNow)
            });
            model.Id = id;
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

        return model;
    }

    public ProductGroup Save(ProductGroupDim dim)
    {
        if (dim.Id > 0)
        {
            var group = this.Find(new FindProductGroupRule() { Id = dim.Id })
                .FirstOrDefault();
            
            if (group is null)
                throw new BusinessException(BusinessExceptionEnum.NOT_POSSIBLE_SAVE_THIS_PRODUCT_GROUP);

            return this.Update(new ProductGroup()
            {
                Id = group.Id,
                Name = dim.Name ?? group.Name,
            });
        }

        return this.Insert(new ProductGroup()
        {
            Name = dim.Name ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        });
    }
    
    public ProductGroup Remove(long id)
    {
        var group = this.Find(new() { Id = id }).FirstOrDefault();
        
        if (group is null)
            throw new BusinessException(BusinessExceptionEnum.PRODUCT_GROUP_NOT_EXISTS);

        var client = this.factory.getHibernumClient();
        try
        {
            client.Connect();
            client.Execute(new BancoExecuteArgument()
            {
                Sql = ProductGroupSql.RemoveProductGroupSql,
                Parameter = ParameterCollection.GetInstance()
                    .Add("@id", group.Id)
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

        return group;
    }
}