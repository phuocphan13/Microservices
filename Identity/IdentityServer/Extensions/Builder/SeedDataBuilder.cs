using IdentityServer.Services.Cores;

namespace IdentityServer.Extensions.Builder;

public static class SeedDataBuilder
{
    public static async Task RunSeedDataBuilder(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
        await service.SeedDataForInitializeAsync();
    }
}