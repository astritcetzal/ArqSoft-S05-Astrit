using Citas_App.Application.Interfaces;

using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Citas_App.Web.Controllers
{
    [Authorize(Roles="Admin, Medico")]
    public class PacienteController : Controller
    {
        
        private readonly IPacienteService _pacienteSer;
        
        public PacienteController(IPacienteService paciente)
        {
            _pacienteSer = paciente;
        }

        public IActionResult Index() => View(_pacienteSer.ObtenerTodos());   

        public IActionResult Detalle(int id)
        {
            var paciente = _pacienteSer.ObtenerPorId(id);
            return paciente == null ? NotFound() : View(paciente);
        }
       
        public IActionResult Agregar()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Agregar(Paciente paciente)
        {
            _pacienteSer.Agregar(paciente);
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Eliminar(int id)
        {
            _pacienteSer.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}