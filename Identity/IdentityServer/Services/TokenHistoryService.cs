using IdentityServer.Domain.Entities;
using IdentityServer.Extensions;
using IdentityServer.Models.Token;
using Microsoft.EntityFrameworkCore;
using Platform.Database.Helpers;

namespace IdentityServer.Services;

public interface ITokenHistoryService
{
    Task<bool> SaveAppUserTokenAsync<T>(Guid accountId, TokenTypeEnum type, T accessToken, CancellationToken cancellationToken = default)
        where T: TokenBase, new();

    Task<T?> GetTokenAsync<T>(Guid accountId, TokenTypeEnum type, CancellationToken cancellationToken = default)
        where T : TokenBase, new();
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

    public async Task<T?> GetTokenAsync<T>(Guid accountId, TokenTypeEnum type, CancellationToken cancellationToken)
        where T: TokenBase, new()
    {
        var token = await GetTokenInternalAsync(accountId, type, cancellationToken);

        return token?.ToTokenModel<T>();
    }

    public async Task<bool> SaveAppUserTokenAsync<T>(
        Guid accountId,
        TokenTypeEnum type,
        T token,
        CancellationToken cancellationToken)
        where T: TokenBase, new()
    {
        var tokenEntity = await GetTokenInternalAsync(accountId, type, cancellationToken);

        await SaveAppUserTokenInternalAsync(type, accountId, token, tokenEntity, cancellationToken);

        var isSaveChange = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return isSaveChange;
    }

    private async Task SaveAppUserTokenInternalAsync<T>(TokenTypeEnum tokenType, Guid accountId, T token, TokenHistory? entity, CancellationToken cancellationToken)
        where T : TokenBase
    {
        if (entity is null)
        {
            var data = new TokenHistory()
            {
                ExternalId = Guid.NewGuid(),
                Token = token.Token,
                CreatedBy = accountId.ToString(),
                CreatedDate = DateTime.Now,
                AccountId = accountId,
                IsActive = true,
                ValidFrom = DateTime.Now,
                ValidTo = token.ExpiredAt,
                Type = tokenType
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
            entity.ValidTo = token.ExpiredAt;

            _tokenHistoryRepository.Update(entity);
        }
    }

    private async Task<TokenHistory?> GetTokenInternalAsync(Guid accountId, TokenTypeEnum type, CancellationToken cancellationToken)
    {
        var token = await _tokenHistoryRepository.Query(x => x.IsActive && x.AccountId == accountId && x.Type == type)
            .OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(cancellationToken);

        return token;
    }
}