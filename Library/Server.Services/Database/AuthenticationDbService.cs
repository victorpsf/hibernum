using Microsoft.EntityFrameworkCore;
using Server.Database.Contexts;
using Server.Database.Entity;
using Server.Services.Database.Rules;

namespace Server.Services.Database;

public class AuthenticationDbService
{
    private readonly AuthenticationContext ctx;

    public AuthenticationDbService(AuthenticationContext ctx)
        => this.ctx = ctx;

    public AuthEntity? Find(FindUserRule rule)
    {
        if (
            (rule.Id == 0) &&
            string.IsNullOrEmpty(rule.Name) &&
            string.IsNullOrEmpty(rule.Email)
        ) return null;

        var queryable = this.ctx.Auth.AsQueryable();

        if (rule.Id > 0)
            queryable = queryable.Where(a => a.Id == rule.Id);
            
        if (!string.IsNullOrEmpty(rule.Name))
            queryable = queryable.Where(a => a.Name == rule.Name);

        if (!string.IsNullOrEmpty(rule.Email))
            queryable = queryable.Where(a => a.Email == rule.Email);

        return queryable.FirstOrDefault();
    }

    public CompanyEntity? Find(FindCompanyRule rule)
    {
        var queryable = this.ctx.Company.AsQueryable();

        if (rule.Id > 0)
            queryable = queryable.Where(a => a.Id == rule.Id);
        else
            return null;

        return queryable.FirstOrDefault();
    }

    public List<CompanyEntity> ListCompany(AuthEntity entity)
        => this.ctx.AuthCompany.AsQueryable()
            .Include(a => a.Auth)
            .Include(a => a.Company)
            .Where(a => a.Auth.Id == entity.Id)
            .ToList()
            .Select(a => a.Company)
            .ToList();
}