using Citas_App.Interfaces;
using Citas_App.Models;
using CitasApp.Models;
using System.Text.Json;

namespace Citas_App.Repositories
{
    public class JsonCitaRepository : ICitaRepository
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonCitaRepository(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "data", "citas.json");
        }
        public List<Cita> ObtenerTodos()
        {
            if (!File.Exists(_path)) return new();
            var json = File.ReadAllText(_path);
            var citasJson = JsonSerializer.Deserialize<List<CitaJson>>(json, _options) ?? new();
            return citasJson.Select(c => new Cita
            {
                Id = c.Id,
                PacienteId = c.PacienteId,
                MedicoId = c.MedicoId,
                Fecha = DateOnly.Parse(c.Fecha),
                Hora = TimeOnly.Parse(c.Hora),
                Motivo = c.Motivo,
                Estado = c.Estado
            }).ToList();
        }
        public Cita? ObtenerPorPaciente(int pacienteId)  
        {
            return ObtenerTodos().FirstOrDefault(c => c.PacienteId == pacienteId);
    }
        public void Agregar(Cita cita)
        {
            var citas = ObtenerTodos();

            // Auto-incrementar el Id
            cita.Id = citas.Count > 0
                      ? citas.Max(i => i.Id) + 1
                      : 1;

            citas.Add(cita);
            Guardar(citas);
        }

        // Método privado: serializa y escribe el archivo
        private void Guardar(List<Cita> citas)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true   // JSON legible para humanos
            };
            var json = JsonSerializer.Serialize(citas, opciones);
            File.WriteAllText(_path, json);
        }

    }
}
