using IdentityServer.Extensions.Builder;
using Platform;
using Platform.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions(builder.Configuration)
    .AddPlatformCommonServices()
    .AddServiceDependency()
    .AddDatabase(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var isRebuildSchema = bool.Parse(builder.Configuration.GetConfigurationValue("Configuration:IsRebuildSchema"));
if (isRebuildSchema)
{
    MigrationBuilder.RunMigrationBuilder(app);
}

var isSeedData = bool.Parse(builder.Configuration.GetConfigurationValue("Configuration:IsSeedData"));
if (isSeedData)
{
    await SeedDataBuilder.RunSeedDataBuilder(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();