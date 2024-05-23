using IdentityServer.Domain;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Extensions.Builder;

public static class MigrationBuilder
{
    public static void RunMigrationBuilder(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AuthenContext>();
        context.Database.Migrate();
    }
}