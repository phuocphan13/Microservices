using Logging.Extensions;
using Logging.Extensions.Configurations;
using Logging.Middlewares;
using Worker;
using Platform;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPlatformCommonServices()
    .AddWorkerServices(builder.Configuration)
    .AddServiceDependencies()
    .AddThirdParties(builder.Configuration)
    .AddOptions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseMigrationMiddleware();
app.UseHttpsRedirection();

app.Run();