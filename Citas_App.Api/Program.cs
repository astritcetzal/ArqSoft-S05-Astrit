using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
using Citas_App.Infrastructure.Observers; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// ── 1. Ruta compartida de SQLite (Apunta a la carpeta de la Web) ──
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "..", "Citas_App", "data");
var sqlitePath = Path.Combine(dataFolder, "citasapp.db");

// ── 2. Inyección de Repositorios (Igual que en la Web, pero con Decorator) ──

// Pacientes: Usamos SQLite y lo envolvemos con el Decorator que pide el profe
builder.Services.AddScoped<IPacienteRepository>(sp =>
{
    var repo = new SqlitePacienteRepository(sqlitePath);
    return new LoggingPacienteRepository(repo);
});

// Médicos y Citas: Directo a SQLite
builder.Services.AddScoped<IMedicoRepository>(_ => new SqliteMedicoRepository(sqlitePath));
builder.Services.AddScoped<ICitaRepository>(_ => new SqliteCitaRepository(sqlitePath));

// ── 3. Observers ──
// Descomenta esto si tienes creados el SmsObserver y EmailObserver
builder.Services.AddScoped<ICitaObserver, SmsObserver>();
builder.Services.AddScoped<ICitaObserver, EmailObserver>();

// ── 4. Servicios de aplicación ──
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

// ── 5. CORS ──
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", app =>
    {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("PermitirTodo");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();