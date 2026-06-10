using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
using Citas_App.Domain.Models;
var builder = WebApplication.CreateBuilder(args);

// ── 1. Carpeta de datos ───────────────────────────────────────────────────────
// Usamos ContentRootPath para que coincida exactamente con la ruta de tus JSON originales
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataFolder);

var csvPacientes = Path.Combine(dataFolder, "pacientes.csv");
var csvMedicos = Path.Combine(dataFolder, "medicos.csv");
var csvCitas = Path.Combine(dataFolder, "citas.csv");

var sqlitePath = Path.Combine(dataFolder, "citasapp.db");

// ── 2. Elige tus Adapters (Comenta y descomenta para tu demostración) ─────────

// ▶ Bloque A — JSON 
/*
builder.Services.AddScoped<IPacienteRepository, JsonPacienteRepository>();
builder.Services.AddScoped<IMedicoRepository,   JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository,     JsonCitaRepository>();
*/

// ▶ Bloque B — CSV  ← ACTIVO AHORA
builder.Services.AddScoped<IPacienteRepository>(sp => (IPacienteRepository)new CsvPacienteRepository(csvPacientes));
builder.Services.AddScoped<IMedicoRepository>(sp => (IMedicoRepository)new CsvMedicoRepository(csvMedicos));
builder.Services.AddScoped<ICitaRepository>(sp => (ICitaRepository)new CsvCitaRepository(csvCitas));

// ▶ Bloque C — SQLite
/*
builder.Services.AddScoped<IPacienteRepository>(_ => new SqlitePacienteRepository(sqlitePath));
builder.Services.AddScoped<IMedicoRepository>  (_ => new SqliteMedicoRepository(sqlitePath));
builder.Services.AddScoped<ICitaRepository>    (_ => new SqliteCitaRepository(sqlitePath));
*/

// ── 3. MVC ────────────────────────────────────────────────────────────────────
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