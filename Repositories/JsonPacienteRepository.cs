using Citas_App.Interfaces;
using Citas_App.Models;
using System.Text.Json;
namespace Citas_App.Repositories
{
    public class JsonPacienteRepository : IPacienteRepository
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonPacienteRepository(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "data", "pacientes.json");
        }



        public List<Paciente> ObtenerTodos()
        {
            if (!File.Exists(_path))
                return new List<Paciente>();

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<Paciente>>(json) ?? new List<Paciente>();
        }

        public Paciente? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(c => c.Id == id);

        }
        public void Agregar(Paciente paciente)
        {
            var pacientes = ObtenerTodos();

            // Auto-incrementar el Id
            paciente.Id = pacientes.Count > 0
                      ? pacientes.Max(i => i.Id) + 1
                      : 1;

            pacientes.Add(paciente);
            Guardar(pacientes);
        }

        private void Guardar(List<Paciente> pacientes)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true   // JSON legible para humanos
            };
            var json = JsonSerializer.Serialize(pacientes, opciones);
            File.WriteAllText(_path, json);
        }

    }
}
