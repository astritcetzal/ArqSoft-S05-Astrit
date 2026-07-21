using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
namespace Citas_App.Infrastructure.Repositories
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
            var citasJson = LeerArchivo();
            return citasJson;
        }
        public Cita? ObtenerPorPaciente(int pacienteId)  
        {
            return ObtenerTodos().FirstOrDefault(c => c.PacienteId == pacienteId);
    }

        public Cita? ObtenerPorId(int idCita)
        {
            return ObtenerTodos().FirstOrDefault(c => c.Id == idCita);

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

        private List<Cita> LeerArchivo()
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

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public void ConfirmarCita(int id)
        {
            throw new NotImplementedException();
        }
    }
}
