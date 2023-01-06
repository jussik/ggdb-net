using GgdbNet.Server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? dbPath = builder.Configuration["DatabasePath"];
if(string.IsNullOrWhiteSpace(dbPath))
    throw new InvalidOperationException("DatabasePath not configured");

dbPath = Environment.ExpandEnvironmentVariables(dbPath);

// Add services to the container.
builder.Services.AddDbContext<GgdbContext>(opts => opts.UseSqlite("Data Source=" + dbPath));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
        .LogInformation("Database persisted at: {DbPath}", dbPath);
    Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
    scope.ServiceProvider.GetRequiredService<GgdbContext>().Database.Migrate();
}

app.Run();
