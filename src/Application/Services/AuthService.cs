using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;

    public AuthService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"] ?? "sua-chave-super-secreta-muito-longa-para-256bits";
        _issuer = configuration["Jwt:Issuer"] ?? "GestaoAutoRepara";
        _audience = configuration["Jwt:Audience"] ?? "GestaoAutoReparaUsers";
        _expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
    }

    public string GerarToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidarCredenciais(string username, string password)
    {
        // MVP: credenciais hardcoded simples
        return username == "admin" && password == "admin123";
    }
}
