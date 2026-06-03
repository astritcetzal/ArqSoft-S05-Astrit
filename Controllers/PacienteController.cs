using Microsoft.AspNetCore.Mvc;
using Citas_App.Models;
using Citas_App.Interfaces;
namespace Citas_App.Controllers
{
    public class PacienteController : Controller
    {
        
        private readonly IPacienteRepository _pacienteRepository;
        
        public PacienteController(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public IActionResult Index() => View(_pacienteRepository.ObtenerTodos());   

        public IActionResult Detalle(int id)
        {
            var paciente = _pacienteRepository.ObtenerPorId(id);
            return paciente == null ? NotFound() : View(paciente);
        }
    }
}