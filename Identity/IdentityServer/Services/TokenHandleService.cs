using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain.Entities;
using IdentityServer.Models.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Platform.Configurations.Options;

namespace IdentityServer.Services;

public interface ITokenHandleService
{
    Task<LoginResponse?> LoginAsync(Account account, CancellationToken cancellationToken = default);
    Task<AccessTokenDetail?> GenerateAccessTokenByRefreshTokenAsync(Account account, string refreshToken, CancellationToken cancellationToken = default);
    Task<bool> ValidateAccessTokenAsync(Guid accountId, TokenTypeEnum type, string token, CancellationToken cancellationToken = default);
}

public class TokenHandleService : ITokenHandleService
{
    private readonly ITokenHistoryService _tokenHistoryService;
    private readonly IRoleService _roleService;
    private readonly JwtSettingsOptions _jwtSettings;

    public TokenHandleService(ITokenHistoryService tokenHistoryService, IRoleService roleService, IOptions<JwtSettingsOptions> jwtSettings)
    {
        ArgumentNullException.ThrowIfNull(jwtSettings);
        
        _tokenHistoryService = tokenHistoryService;
        _roleService = roleService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<bool> ValidateAccessTokenAsync(Guid accountId, TokenTypeEnum type, string token, CancellationToken cancellationToken)
    {
        var tokenEntity = await _tokenHistoryService.GetTokenAsync<AccessTokenModel>(accountId, type, cancellationToken);

        return ValidateTokenInternal(tokenEntity, token);
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid accountId, TokenTypeEnum type, string token, CancellationToken cancellationToken)
    {
        var tokenEntity = await _tokenHistoryService.GetTokenAsync<RefreshTokenModel>(accountId, type, cancellationToken);

        return ValidateTokenInternal(tokenEntity, token);
    }

    public async Task<AccessTokenDetail?> GenerateAccessTokenByRefreshTokenAsync(Account account, string refreshToken, CancellationToken cancellationToken)
    {
        var isValid = await ValidateRefreshTokenAsync(account.Id, TokenTypeEnum.RefreshToken, refreshToken, cancellationToken);

        if (!isValid)
        {
            return null;
        }
        
        var token = await GenerateAccessTokenInternal(account.Id.ToString(), account.Email);

        return new AccessTokenDetail()
        {
            Token = token.Token,
            ExpiredAt = (token.ExpiredAt - DateTime.Now).Minutes
        };
    }

    public async Task<LoginResponse?> LoginAsync(Account account, CancellationToken cancellationToken)
    {
        var accessToken = await GenerateAccessTokenInternal(account.Id.ToString(), account.Email);
        var refreshToken = GenerateRefreshTokenInternal();
        
        await _tokenHistoryService.SaveAppUserTokenAsync(account.Id, TokenTypeEnum.AccessToken, accessToken, cancellationToken);

        await _tokenHistoryService.SaveAppUserTokenAsync(account.Id, TokenTypeEnum.RefreshToken, refreshToken, cancellationToken);

        return new()
        {
            AccessToken = accessToken.Token,
            AccessTokenExpires = (accessToken.ExpiredAt - DateTime.Now).Minutes,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpires = (refreshToken.ExpiredAt - DateTime.Now).Minutes,
            TokenType = "Bearer"
        };
    }

    #region Internal Functions
    private async Task<AccessTokenModel> GenerateAccessTokenInternal(string accountId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var lifeTime = TimeSpan.FromHours(_jwtSettings.LifeTime);

        var roles = await _roleService.GetRoleByUserIdAsync(accountId);

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email),
            new("userId", accountId),
            new("role", string.Join("|", roles.Select(x => x.Name)))
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(lifeTime),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
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

    private bool ValidateTokenInternal<T>(T? tokenModel, string token)
        where T : TokenBase
    {
        if (tokenModel is null)
        {
            return false;
        }

        if (tokenModel.Token != token)
        {
            return false;
        }

        if (tokenModel.ExpiredAt < DateTime.Now)
        {
            return false;
        }

        return true;
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
    #endregion
}