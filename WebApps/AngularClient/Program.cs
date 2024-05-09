using AngularClient.Extensions;
using ApiClient;
using Platform;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services
    .AddPlatformCommonServices()
    .AddCommonServices()
    .AddCatalogServices()
    .AddIdentityServerServices()
    .AddDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(b => b.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();