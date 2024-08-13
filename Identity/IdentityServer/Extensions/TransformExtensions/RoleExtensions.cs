using ApiClient.IdentityServer.Models.Response;
using IdentityServer.Domain.Entities;

namespace IdentityServer.Extensions.TransformExtensions;

public static class RoleExtensions
{
     public static RoleDetail ToDetail(this Role role)
     {
          return new()
          {
               ExternalId = role.Id.ToString(),
               Name = role.Name,
               Description = role.Description ?? string.Empty
          };
     }
}