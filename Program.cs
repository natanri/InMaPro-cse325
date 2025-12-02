using InMaPro_cse325.Data;
using Microsoft.EntityFrameworkCore;
using InMaPro_cse325.Components;

var builder = WebApplication.CreateBuilder(args);

// EF Core con SQLite (estable para .NET 8)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=inmapro.db"));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
