using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        string issuer = jwtSettings["Issuer"]!;
        string audience = jwtSettings["Audience"]!;
        string secretKey = jwtSettings["SecretKey"]!;

        int expiryMinutes =
            int.Parse(jwtSettings["ExpiryMinutes"]!);

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        // Information stored inside the token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

        DateTime expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        string generatedToken =
            new JwtSecurityTokenHandler().WriteToken(token);

        return (generatedToken, expiresAt);
    }
}