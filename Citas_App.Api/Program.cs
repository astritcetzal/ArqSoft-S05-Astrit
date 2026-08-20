using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
using Citas_App.Infrastructure.Observers;

SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ── SWAGGER: Registro de servicios ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── 1. Ruta compartida de SQLite ──
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "..", "Citas_App", "data");
var sqlitePath = Path.Combine(dataFolder, "citasapp.db");

// ── 2. Inyección de Repositorios ──
builder.Services.AddScoped<IPacienteRepository>(sp =>
{
    var repo = new SqlitePacienteRepository(sqlitePath);
    return new LoggingPacienteRepository(repo);
});

builder.Services.AddScoped<IMedicoRepository>(_ => new SqliteMedicoRepository(sqlitePath));
builder.Services.AddScoped<ICitaRepository>(_ => new SqliteCitaRepository(sqlitePath));

// ── 3. Observers ──
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

// ── SWAGGER: Middleware ──
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CitasApp API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("PermitirTodo");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();