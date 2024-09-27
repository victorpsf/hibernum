using System.Collections.ObjectModel;
using Server.Database.Contexts;
using Server.Database.Entity;
using Server.Services.Database.Rules;
using Server.Extensions;
using System.Data.Entity;
using Server.Dtos;

namespace Server.Services.Database;

public class HibernumDbService
{
    private readonly HibernumContext ctx;

    public HibernumDbService(HibernumContext ctx)
        => this.ctx = ctx;

    public List<ProductEntity> Find(FindProductRule rule)
    {
        var values = this.ctx.Product
            .Where(a => (rule.Id == null || a.Id == rule.Id) && (rule.Name == null || a.Name.ToUpper().Contains(rule.Name.ToLike().ToUpper())) && (rule.Size == null || a.Size.ToUpper() == rule.Size.ToUpper()))
            .ToList();

        foreach (var value in values)
        {
            this.ctx.Entry(value).Collection(v => v.Descriptions).Load();
            this.ctx.Entry(value).Collection(v => v.Types).Load();
            this.ctx.Entry(value).Reference(v => v.Group).Load();

            value.Descriptions = new(value.Descriptions.Where(a => a.DeletedAt == null).ToList());
            value.Types = new(value.Types.Where(a => a.DeletedAt == null).ToList());
        }

        return values
            .ToList();
    }

    private ProductEntity Update(ProductDTO dto, ProductEntity entity)
    {
        foreach (var type in entity.Types)
        { type.DeletedAt = DateTime.Now; }

        foreach (var desc in entity.Descriptions)
        { desc.DeletedAt = DateTime.Now; }

        entity.Update(dto.ToEntity());
        dto.ToEntity().Types.Select(a => { a.Id = 0; return a; }).ToList().ForEach(a => entity.Types.Add(a));
        dto.ToEntity().Descriptions.Select(a => { a.Id = 0; return a; }).ToList().ForEach(a => entity.Descriptions.Add(a));
        
        foreach (var type in entity.Types)
            if (type.Id == 0) this.ctx.ProductType.Add(type);
            else this.ctx.ProductType.Update(type);

        foreach (var desc in entity.Descriptions)
            if (desc.Id == 0) this.ctx.ProductDescription.Add(desc);
            else this.ctx.ProductDescription.Update(desc);

        if (entity.Group is not null && entity.Group.Id == 0)
            this.ctx.ProductGroup.Add(entity.Group);
        
        this.ctx.Product.Update(entity);
        this.ctx.SaveChanges();

        entity.Descriptions = new(entity.Descriptions.Where(a => a.DeletedAt == null).ToList());
        entity.Types = new(entity.Types.Where(a => a.DeletedAt == null).ToList());
        return entity;
    } 
    
    private ProductEntity Insert(ProductDTO dto)
    {
        var entity = dto.ToEntity();

        var types = entity.Types.Where(a => a.Id == 0).ToList();
        var typesCreated = entity.Types.Where(a => a.Id > 0).ToList();
        foreach (var type in typesCreated)
        {
            type.DeletedAt = DateTime.Now;
            this.ctx.ProductType.Update(type);
        }
        entity.Types.Clear();
        types.AddRange(typesCreated.Select(a => { a.Id = 0; return a; }));
        entity.Types = new(types);

        var descriptions = entity.Descriptions.Where(a => a.Id == 0).ToList();
        var descriptionsCreated = entity.Descriptions.Where(a => a.Id > 0).ToList();
        foreach (var desc in descriptionsCreated)
        {
            desc.DeletedAt = DateTime.Now;
            this.ctx.ProductDescription.Update(desc);
        }
        entity.Descriptions.Clear();
        descriptions.AddRange(descriptionsCreated.Select(a => { a.Id = 0; return a; }));
        entity.Descriptions = new(descriptions);
            
        entity.Update(dto.ToEntity());
        foreach (var type in entity.Types)
            this.ctx.ProductType.Add(type);

        foreach (var desc in entity.Descriptions)
            this.ctx.ProductDescription.Add(desc);

        if (entity.Group is not null && entity.Group.Id == 0)
            this.ctx.ProductGroup.Add(entity.Group);

        this.ctx.Product.Add(entity);
        this.ctx.SaveChanges();
        return entity;
    } 

    public ProductEntity Save(ProductDTO dto)
    {
        ProductEntity? entity = null;
        if (dto.Id is not null)
        {
            var results = this.Find(new FindProductRule() { Id = dto.Id });

            if (!results.Any() || results.Count > 1)
                throw new Exception();

            entity = results.FirstOrDefault();
        }

        return (entity is not null)
            ? this.Update(dto, entity)
            : this.Insert(dto);
    }
}