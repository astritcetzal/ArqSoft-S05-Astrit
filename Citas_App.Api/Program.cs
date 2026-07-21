using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Repositorios
//builder.Services.AddScoped<IPacienteRepository, MemoriaPacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();
//usando el Factory y envolviendo con el Decorator
builder.Services.AddScoped<IPacienteRepository>( sp =>
    {
    var env = sp.GetRequiredService<IWebHostEnvironment>();
        var repo = RepositoryFactory.CrearPacienteRepository(builder.Environment.EnvironmentName, env);
    return new LoggingPacienteRepository(repo);
});

builder.Services.AddScoped<ICitaObserver, SmsObserver>();
builder.Services.AddScoped<ICitaObserver, EmailObserver>();
// Servicios de aplicación
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

//nuevooo
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
// AGREGAR ESTO: Activar CORS
app.UseCors("PermitirTodo");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
