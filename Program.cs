using InMaPro_cse325.Data;
using InMaPro_cse325.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de componentes Razor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=inmapro.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Registrar servicios personalizados
builder.Services.AddScoped<DashboardService>();

var app = builder.Build();

// Configurar pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Mapear componentes Razor - usa el tipo correcto para tu componente raíz
app.MapRazorComponents<InMaPro_cse325.Components.App>()  // ¡CORREGIDO!
    .AddInteractiveServerRenderMode();

// Inicializar base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        
        // Inicializar datos
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error inicializando base de datos");
    }
}

app.Run();