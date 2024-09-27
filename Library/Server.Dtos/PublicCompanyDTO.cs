using Server.Database.Entity;

namespace Server.Dtos;

public class PublicCompanyDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static PublicCompanyDTO? ByEntity(CompanyEntity? entity)
    {
        if (entity is null)
            return null;

        return new()
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}