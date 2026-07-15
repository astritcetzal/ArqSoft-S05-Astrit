using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
using Citas_App.Domain.Models;
using Citas_App.Application.Services;
using Citas_App.Application.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// ── 1. Carpeta de datos ───────────────────────────────────────────────────────
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataFolder);

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
