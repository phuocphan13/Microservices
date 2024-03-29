using IdentityServer.Common;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder
    .Host
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", false, true);
    })
    .ConfigureLogging((hostingContext, logginBuilder) =>
    {
        logginBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        logginBuilder.AddConsole();
        logginBuilder.AddDebug();
    });

builder.Services
    .AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

builder.Services.AddCustomAuthenticate(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.UseAuthorization();

app.Run();
