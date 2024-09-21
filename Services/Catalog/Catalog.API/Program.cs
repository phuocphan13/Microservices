using ApiClient;
using Catalog.API.Common.Extensions;
using Catalog.API.Extensions.AppBuilder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using OpenTelemetryFramework;
using Platform;
using Worker;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHealthChecks();

var isRebuildSchema = builder.Configuration.GetValue<bool>(Platform.Constants.DatabaseConst.ConnectionSetting.MongoDB.IsRebuildSchema);

// Add services to the container.

builder.Services.AddControllers();

builder.AddOpenTelemetryLogs();

builder.Services
    .AddPlatformCommonServices()
    .AddIdentityInternalClient()
    .AddServiceDependency()
    .AddThirdParty(builder.Configuration)
    .AddRedisServices(builder.Configuration)
    .AddWorkerServices(builder.Configuration)
    .AddOptions(builder.Configuration)
    .AddOpenTelemetryTracing(builder.Configuration)
    .AddOpenTelemetryMetrics(builder.Configuration);

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys\"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

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

await app.InitializePlatformDbContextsAsync(builder.Configuration, isRebuildSchema);

await app.RunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
