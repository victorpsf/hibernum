using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Server.Models.Security;
using Server.Properties;

namespace Server.Services;

public class JwtSecurity
{
    private ServerProperties serverProperties;

    public JwtSecurity(ServerProperties serverProperties)
        => this.serverProperties = serverProperties;

    public SymmetricSecurityKey SymmetricSecurityKey { get => new(this.serverProperties.SecurityProperties.TokenSecret); }
    public SigningCredentials SigningCredentials { get => new(this.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature); }

    public UserToken Write(ClaimIdentifier claim)
    {
        var now = DateTime.UtcNow;
        Console.WriteLine(now);
        Console.WriteLine(now.AddMinutes(this.serverProperties.SecurityProperties.TokenMinutes));
        var handler = new JwtSecurityTokenHandler();

        return new UserToken()
        {
            Token = handler.WriteToken(
                handler.CreateToken(
                    new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(
                            new Claim[]
                            { new("aI", claim.Id.ToString()), new("cI", claim.Company.ToString()) }
                        ),
                        Issuer = this.serverProperties.SecurityProperties.TokenIssuer,
                        TokenType = this.serverProperties.SecurityProperties.TokenType,
                        Expires = now.AddMinutes(this.serverProperties.SecurityProperties.TokenMinutes),
                        SigningCredentials = this.SigningCredentials
                    })
            ),
            Type = this.serverProperties.SecurityProperties.TokenType
        };
    }

    public ClaimIdentifier Read(string token)
    {
        try
        {
            (new JwtSecurityTokenHandler())
                .ValidateToken(
                    token, 
                    new TokenValidationParameters
                    {
                        ValidIssuer = this.serverProperties.SecurityProperties.TokenIssuer,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        IssuerSigningKey = this.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true
                    }, 
                    out SecurityToken validatedToken
                );

            var jwt = (JwtSecurityToken)validatedToken;
            return ClaimIdentifier.GetInstance(jwt.Claims);
        }
        catch (Exception error)
        { throw new UnauthorizedAccessException($"INVALID TOKEN {error.Message}", error); }
    }
}