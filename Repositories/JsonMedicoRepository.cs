using Citas_App.Interfaces;
using Citas_App.Models;
using System.Text.Json;
namespace Citas_App.Repositories

{
    public class JsonMedicoRepository : IMedicoRepository
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonMedicoRepository(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "data", "medicos.json");
        }

        public List<Medico> ObtenerTodos()
        {
            if (!File.Exists(_path))
                return new();

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<Medico>>(json) ?? new();
        }
        public Medico? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(c => c.Id == id);

        }
    }
}
