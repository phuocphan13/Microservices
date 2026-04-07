using ApiClient;
using Catalog.API.Extensions.AppBuilder;
using Platform;
using CatalogExtension = Catalog.API.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddPlatformCommonServices()
    .AddIdentityInternalClient()
    .AddServiceDependency()
    .AddThirdParty(builder.Configuration)
    .AddRedisServices(builder.Configuration)
    // .AddWorkerServices(builder.Configuration)
    .AddOptions(builder.Configuration);

CatalogExtension.InitializeDB
    .InitializePlatformDbContextsAsync(builder.Configuration)
    .GetAwaiter()
    .GetResult();

//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys\"))
//    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
//    {
//        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
//        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
//    });

var app = builder.Build();

//Todo adding healthcheck later
// app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapControllers();

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
