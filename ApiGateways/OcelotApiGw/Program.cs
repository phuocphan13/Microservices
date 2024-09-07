using ApiClient;
using IdentityServer.Common;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", false, true);

builder.Services
    .AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

builder.Services
    .AddIdentityInternalClient();

builder.Services.AddCustomAuthenticate(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


await app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.Run();
