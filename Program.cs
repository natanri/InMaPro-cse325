using InMaPro_cse325.Data;
using InMaPro_cse325.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// SOLO servicios esenciales - NADA de autenticación
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=inmapro.db"));

builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Pipeline MÍNIMO - SIN UseAuthentication/UseAuthorization
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();  // ← ÚNICAMENTE esto

app.MapRazorPages();
app.MapBlazorHub();
app.MapRazorComponents<InMaPro_cse325.Components.App>()
    .AddInteractiveServerRenderMode();

// Inicializar BD
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}

app.Run();