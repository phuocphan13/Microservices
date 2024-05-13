using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain.Entities;
using IdentityServer.Models.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Services;

public interface ITokenHandleService
{
    Task<LoginResponse?> LoginAsync(Account account, CancellationToken cancellationToken = default);
    Task<AccessTokenDetail?> GenerateAccessTokenByRefreshTokenAsync(Account account, string refreshToken, CancellationToken cancellationToken = default);
}

public class TokenHandleService : ITokenHandleService
{
    private readonly IConfiguration _configuration;
    private readonly ITokenHistoryService _tokenHistoryService;
    private readonly UserManager<Account> _userManager;

    public TokenHandleService(IConfiguration configuration, ITokenHistoryService tokenHistoryService, UserManager<Account> userManager)
    {
        _configuration = configuration;
        _tokenHistoryService = tokenHistoryService;
        _userManager = userManager;
    }

    public async Task<AccessTokenDetail?> GenerateAccessTokenByRefreshTokenAsync(Account account, string refreshToken, CancellationToken cancellationToken)
    {
        var isValid = await _tokenHistoryService.ValidateTokenAsync(account.Id, TokenTypeEnum.RefreshToken, refreshToken, cancellationToken);

        if (!isValid)
        {
            return null;
        }
        
        var token = GenerateAccessTokenInternal(account.Id.ToString(), account.Email);

        return new AccessTokenDetail()
        {
            Token = token.Token,
            ExpiredAt = (token.ExpiredAt - DateTime.Now).Minutes
        };
    }

    public async Task<LoginResponse?> LoginAsync(Account account, CancellationToken cancellationToken)
    {
        var accessToken = GenerateAccessTokenInternal(account.Id.ToString(), account.Email);
        var refreshToken = GenerateRefreshTokenInternal();;
        
        await _tokenHistoryService.SaveAppUserTokenAsync(account.Id, TokenTypeEnum.AccessToken, accessToken, cancellationToken);

        await _tokenHistoryService.SaveAppUserTokenAsync(account.Id, TokenTypeEnum.AccessToken, refreshToken, cancellationToken);

        return new()
        {
            AccessToken = accessToken.Token,
            AccessTokenExpires = (accessToken.ExpiredAt - DateTime.Now).Minutes,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpires = (refreshToken.ExpiredAt - DateTime.Now).Minutes,
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