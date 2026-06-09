using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
namespace Citas_App.Web.Controllers
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
       
        public IActionResult Agregar()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Agregar(Paciente paciente)
        {
            _pacienteRepository.Agregar(paciente);
            return RedirectToAction("Index");
        }
    }
}