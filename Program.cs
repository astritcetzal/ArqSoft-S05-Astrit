using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
using Citas_App.Domain.Models;
var builder = WebApplication.CreateBuilder(args);

// ── 1. Carpeta de datos ───────────────────────────────────────────────────────
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataFolder);
/*
var csvPacientes = Path.Combine(dataFolder, "pacientes.csv");
var csvMedicos = Path.Combine(dataFolder, "medicos.csv");
var csvCitas = Path.Combine(dataFolder, "citas.csv");
*/
//var sqlitePath = Path.Combine(dataFolder, "citasapp.db");


// ▶ Bloque A — JSON 

builder.Services.AddSingleton<IPacienteRepository, JsonPacienteRepository>();
builder.Services.AddSingleton<IMedicoRepository,   JsonMedicoRepository>();
builder.Services.AddSingleton<ICitaRepository,     JsonCitaRepository>();


// ▶ Bloque B — CSV  ← ACTIVO AHORA
/*
builder.Services.AddSingleton<IPacienteRepository>(sp => (IPacienteRepository)new CsvPacienteRepository(csvPacientes));
builder.Services.AddSingleton<IMedicoRepository>(sp => (IMedicoRepository)new CsvMedicoRepository(csvMedicos));
builder.Services.AddSingleton<ICitaRepository>(sp => (ICitaRepository)new CsvCitaRepository(csvCitas));
*/

// ▶ Bloque C — SQLite
/*
builder.Services.AddSingleton<IPacienteRepository>(_ => new SqlitePacienteRepository(sqlitePath));
builder.Services.AddSingleton<IMedicoRepository>  (_ => new SqliteMedicoRepository(sqlitePath));
builder.Services.AddSingleton<ICitaRepository>    (_ => new SqliteCitaRepository(sqlitePath));
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