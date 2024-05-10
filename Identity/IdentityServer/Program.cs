using IdentityServer.Domain;
using IdentityServer.Domain.Entities;
using IdentityServer.Domain.Helpers;
using IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Platform.Database.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthenContext>();
builder.Services.AddIdentity<Account, Role>()
    .AddEntityFrameworkStores<AuthenContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
builder.Services.AddScoped<ITokenHandleService, TokenHandleService>();
builder.Services.AddScoped<ITokenHistoryService, TokenHistoryService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();