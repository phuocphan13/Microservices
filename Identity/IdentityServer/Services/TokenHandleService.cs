using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiClient.IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services;

public interface ITokenHandleService
{
    TokenResponse GenerateTokenAsync(GenerateTokenRequest request);
}

public class TokenHandleService : ITokenHandleService
{
    private readonly IConfiguration _configuration;

    public TokenHandleService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponse GenerateTokenAsync(GenerateTokenRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
        var lifeTime = TimeSpan.FromHours(int.Parse(_configuration["JwtSettings:LifeTime"]));
        
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email!),
            new(JwtRegisteredClaimNames.Email, request.Email!),
            new("userId", request.UserId!),
        };

        // foreach (var claimPair in request.CustomClaims)
        // {
        //     var valueType = ClaimValueTypes.String;
        //     
        //     if (int.TryParse(claimPair.Value, out _))
        //     {
        //         valueType = ClaimValueTypes.Double;
        //     }
        //     else if (bool.TryParse(claimPair.Value, out _))
        //     {
        //         valueType = ClaimValueTypes.Boolean;
        //     }
        //     
        //     var claim = new Claim(claimPair.Key, claimPair.Value, valueType);
        //     
        //     claims.Add(claim);
        // }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(lifeTime),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return new()
        {
            AccessToken = token
        };
    }
}