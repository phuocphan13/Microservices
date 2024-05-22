using IdentityServer.Domain.Entities;
using IdentityServer.Models.Token;

namespace IdentityServer.Extensions;

public static class TokenHistoryExtensions
{
    public static T ToTokenModel<T>(this TokenHistory entity)
        where T : TokenBase, new()
    {
        return new T()
        {
            Token = entity.Token,
            ExpiredAt = entity.ValidTo
        };
    }
}