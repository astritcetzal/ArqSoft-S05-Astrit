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

        [Authorize(Roles = "Admin")]
        public IActionResult Agregar()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Editar(int id)
        {
            var paciente = _pacienteSer.ObtenerPorId(id);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Editar(Paciente paciente)
        {
            if (!ModelState.IsValid)
                return View(paciente);

            _pacienteSer.Editar(paciente);

            return RedirectToAction("Detalle", new { id = paciente.Id });
        }
    }
}