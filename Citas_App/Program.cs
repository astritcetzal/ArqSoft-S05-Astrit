using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Citas_App.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// ── 1. Carpeta de datos ───────────────────────────────────────────────────────
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataFolder);
//var appEnv = builder
// Rutas para CSV
/*
var csvPacientes = Path.Combine(dataFolder, "pacientes.csv");
var csvMedicos = Path.Combine(dataFolder, "medicos.csv");
var csvCitas = Path.Combine(dataFolder, "citas.csv");
*/
// Ruta para SQLite (un solo archivo .db para las 3 tablas)
var sqlitePath = Path.Combine(dataFolder, "citasapp.db");
if (!Directory.Exists(Path.GetDirectoryName(sqlitePath)))
{
    Directory.CreateDirectory(Path.GetDirectoryName(sqlitePath)!);
}
// ── 2. Elige tus Adapters ─────────────────────────────────────────────────────
// Descomenta el bloque que quieras y comenta los otros dos.
// ¡Las interfaces (Ports) no cambian!

// ▶ Bloque A — JSON (como estaba antes)
/*
builder.Services.AddSingleton<IPacienteRepository, JsonPacienteRepository>();
builder.Services.AddSingleton<IMedicoRepository,   JsonMedicoRepository>();
builder.Services.AddSingleton<ICitaRepository,     JsonCitaRepository>();

*/
// ▶ Bloque B — CSV  ← activo ahora
/*
builder.Services.AddSingleton<IPacienteRepository>(sp => (IPacienteRepository)new CsvPacienteRepository(csvPacientes));
builder.Services.AddSingleton<IMedicoRepository>(sp => (IMedicoRepository)new CsvMedicoRepository(csvMedicos));
builder.Services.AddSingleton<ICitaRepository>(sp => (ICitaRepository)new CsvCitaRepository(csvCitas));
*/
// ▶ Bloque C — SQLite Corregido
builder.Services.AddScoped<IPacienteRepository>(_ => new SqlitePacienteRepository(sqlitePath));
builder.Services.AddScoped<IMedicoRepository>(_ => new SqliteMedicoRepository(sqlitePath));
builder.Services.AddScoped<ICitaRepository>(_ => new SqliteCitaRepository(sqlitePath));

// ── 3. Servicios de aplicación (no cambian con el Adapter) ───────────────────
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<ICitaService, CitaService>();

// ── 4. MVC ────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// DATABASES & IDENTITY INFRASTRUCTURE (SQLite Setup)

// Inyectamos el DbContext indicándole que use SQLite y dónde guardar las migraciones
builder.Services.AddDbContext<CitasDbContext>(options =>
    options.UseSqlite($"Data Source={sqlitePath}",
        b => b.MigrationsAssembly("Citas_App.Infrastructure")
    )
);

// Configuración de Identity (Esta es igual a la de tu profe)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<CitasDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login"; // Asegúrate de que apunte a tu controlador Cuenta
    options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Los 3 roles de nuestro sistema
    string[] roles = { "Admin", "Medico", "Paciente" };

    foreach (var rol in roles)
    {
        // Si el rol no existe en la base de datos, lo crea
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }
}

app.Run(); 