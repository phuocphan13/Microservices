using IdentityServer.Domain.Entities;
using IdentityServer.Models.Token;
using Microsoft.EntityFrameworkCore;
using Platform.Database.Helpers;

namespace IdentityServer.Services;

public interface ITokenHistoryService
{
    Task<bool> SaveAppUserTokenAsync(Guid accountId, AccessTokenModel? accessToken = null!, RefreshTokenModel? refreshToken = null!, CancellationToken cancellationToken = default);
}

public class TokenHistoryService : ITokenHistoryService
{
    private readonly IRepository<TokenHistory> _tokenHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TokenHistoryService(IRepository<TokenHistory> tokenHistoryRepository, IUnitOfWork unitOfWork)
    {
        _tokenHistoryRepository = tokenHistoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> SaveAppUserTokenAsync(
        Guid accountId, AccessTokenModel? accessToken = null!,
        RefreshTokenModel? refreshToken = null!,
        CancellationToken cancellationToken = default)
    {
        var tokens = await _tokenHistoryRepository.Query(x => x.IsActive && x.AccountId == accountId)
            .ToListAsync(cancellationToken);

        if (accessToken is not null)
        {
            var tokenEntity = tokens.FirstOrDefault(x => x.Type == TokenTypeEnum.AccessToken);

            await SaveAppUserTokenInternalAsync(TokenTypeEnum.AccessToken, accountId, accessToken, tokenEntity, cancellationToken);
        }

        if (refreshToken is not null)
        {
            var tokenEntity = tokens.FirstOrDefault(x => x.Type == TokenTypeEnum.RefreshToken);

            await SaveAppUserTokenInternalAsync(TokenTypeEnum.RefreshToken, accountId, refreshToken, tokenEntity, cancellationToken);
        }

        var isSaveChange = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return isSaveChange;
    }

    private async Task SaveAppUserTokenInternalAsync<T>(TokenTypeEnum tokenType, Guid accountId, T token, TokenHistory? entity, CancellationToken cancellationToken)
        where T : TokenBase
    {
        DateTime expiredTime = tokenType switch
        {
            TokenTypeEnum.AccessToken => token.ExpiredAt,
            TokenTypeEnum.RefreshToken => token.ExpiredAt,
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };

        if (entity is null)
        {
            var data = new TokenHistory()
            {
                Token = token.Token,
                CreatedBy = accountId.ToString(),
                CreatedDate = DateTime.Now,
                AccountId = accountId,
                IsActive = true,
                ValidFrom = DateTime.Now,
                ValidTo = expiredTime,
            };

            await _tokenHistoryRepository.InsertAsync(data, cancellationToken);
        }
        else
        {
            entity.Token = token.Token;
            entity.UpdatedBy = accountId.ToString();
            entity.UpdatedDate = DateTime.Now;
            entity.IsActive = true;
            entity.ValidFrom = DateTime.Now;
            entity.ValidTo = expiredTime;

            _tokenHistoryRepository.Update(entity);
        }
    }
}