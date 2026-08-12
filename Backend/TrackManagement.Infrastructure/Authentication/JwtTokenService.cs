using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackManagement.Application.DTOs.Auth;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.Infrastructure.Authentication;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponse GenerateToken(string username)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT key is not configured.");

        var issuer = _configuration["Jwt:Issuer"];

        var audience = _configuration["Jwt:Audience"];

        var expiryMinutes =
            int.TryParse(
                _configuration["Jwt:ExpiryMinutes"],
                out var minutes)
                ? minutes
                : 60;

        var expiresAt =
            DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                username),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),

            new(
                ClaimTypes.Name,
                username)
        };

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new TokenResponse
        {
            AccessToken =
                new JwtSecurityTokenHandler()
                    .WriteToken(token),

            ExpiresAt = expiresAt
        };
    }
}