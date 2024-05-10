using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain.Entities;
using IdentityServer.Models.Token;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services;

public interface ITokenHandleService
{
    Task<LoginResponse?> LoginAsync(Account account, CancellationToken cancellationToken = default);
}

public class TokenHandleService : ITokenHandleService
{
    private readonly IConfiguration _configuration;
    private readonly ITokenHistoryService _tokenHistoryService;

    public TokenHandleService(IConfiguration configuration, ITokenHistoryService tokenHistoryService)
    {
        _configuration = configuration;
        _tokenHistoryService = tokenHistoryService;
    }

    public async Task<LoginResponse?> LoginAsync(Account account, CancellationToken cancellationToken)
    {
        var accessToken = GenerateAccessTokenInternal(account.Id.ToString(), account.Email);
        var refreshToken = GenerateRefreshTokenInternal();

        var result = await _tokenHistoryService.SaveAppUserTokenAsync(account.Id, accessToken, refreshToken, cancellationToken);

        if (!result)
        {
            return null;
        }

        return new()
        {
            AccessToken = accessToken.Token,
            AccessTokenExpires = accessToken.ExpiredAt,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpires = refreshToken.ExpiredAt,
            TokenType = "Bearer"
        };
    }

    private AccessTokenModel GenerateAccessTokenInternal(string accountId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
        var lifeTime = TimeSpan.FromHours(int.Parse(_configuration["JwtSettings:LifeTime"]));
        
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email),
            new("userId", accountId),
        };

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
            Token = token,
            ExpiredAt = tokenDescriptor.Expires!.Value
        };
    }

    private RefreshTokenModel GenerateRefreshTokenInternal()
    {
        return new RefreshTokenModel()
        {
            Token = GenerateRandomCode(),
            ExpiredAt = DateTime.Now.AddDays(30)
        };
    }

    private string GenerateRandomCode()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}