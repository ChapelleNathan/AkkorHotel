using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Helper;

public class AuthHelper(IConfiguration configuration)
{
    public string GenerateToken(User user)
    {
        var tokenKey = configuration.GetSection("AppSettings:Token").Value;
        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new InvalidOperationException("La clé du Token JWT est manquante dans le AppSettings");
        }
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(tokenKey);
        var credential = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credential
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
        return claims;
    }
}