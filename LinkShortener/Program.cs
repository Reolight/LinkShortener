using EFCoreStore;
using LinkShortenerCore.Repository;
using LinkShortenerCore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
if (builder.Configuration.GetConnectionString("UrlContext") is not { } connectionString)
    throw new NullReferenceException(
        "No connection string provided. Provide connection string with name \"UrlContext\" and retry");

builder.Services.AddMySqlWithEfCoreStore(connectionString);

builder.Services.AddScoped<IUrlRepository, EfCoreRepository>();
builder.Services.AddTransient<IUrlShortenerService, UrlShortenerMd5>();

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();