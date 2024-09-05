using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using IdentityServer.Domain.Helpers;
using IdentityServer.Extensions.Builder;
using IdentityServer.Services;
using IdentityServer.Services.Cores;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Platform;
using Platform.Database.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthenContext>(options => options.UseSqlServer(builder.Configuration["Configuration:ConnectionString"]));

builder.Services.AddIdentity<Account, Role>()
    .AddEntityFrameworkStores<AuthenContext>()
    .AddDefaultTokenProviders();

// builder.Services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
//     .AddOAuthValidation();

builder.Services
    .AddOptions(builder.Configuration)
    .AddPlatformCommonServices();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ITokenHandleService, TokenHandleService>();
builder.Services.AddScoped<ITokenHistoryService, TokenHistoryService>();

builder.Services.AddScoped<ISeedDataService, SeedDataService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var isRebuildSchema = bool.Parse(builder.Configuration["Configuration:IsRebuildSchema"]);
if (isRebuildSchema)
{
    MigrationBuilder.RunMigrationBuilder(app);
}

var isSeedData = bool.Parse(builder.Configuration["Configuration:IsSeedData"]);
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