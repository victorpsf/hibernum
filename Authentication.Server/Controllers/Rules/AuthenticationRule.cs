using Server.Validation;

namespace Authentication.Server.Controllers.Rules;

public class AuthenticationRule
{
    [StringValidation(required = false, min = 5, ErrorMessage = "'Name' inválido ou não atende ao requisito minimo de nome")]
    public string? Name { get; set; }
    [EmailValidation(required = false, ErrorMessage = "'Email' inválido ou não informado")]
    public string? Email { get; set; }
    [StringValidation(required = true, min = 8, ErrorMessage = "'Password' não informado ou não atende ao requisito minimo de senha")]
    public string? Password { get; set; }
}