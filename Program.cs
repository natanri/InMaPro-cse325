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

builder.Services.AddScoped<ExportService>();

builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600; // 100MB
});

var app = builder.Build();

// Configurar pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect($"/Error/404");
    }
});

// Mapear componentes Razor - usa el tipo correcto para tu componente raíz
app.MapRazorComponents<InMaPro_cse325.Components.App>() 
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