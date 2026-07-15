using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace Citas_App.Web.Controllers

{
    public class CitaController : Controller
    {
        private readonly ICitaService _citaSer;
        private readonly IPacienteService _pacienteSer;
        private readonly IMedicoService _medicoSer;


        public CitaController(ICitaService cita, IPacienteService paciente, IMedicoService medico)
        {
            _citaSer = cita;
            _pacienteSer = paciente;
            _medicoSer = medico;
        }


        public IActionResult Index()
        {

            ViewBag.Pacientes = _pacienteSer.ObtenerTodos();
            ViewBag.Medicos = _medicoSer.ObtenerTodos();
            return View(_citaSer.ObtenerTodos());
        }

        public IActionResult PorPaciente(int pacienteId)
        {
            var citas = _citaSer.ObtenerTodos().Where(c => c.PacienteId == pacienteId).ToList();
            ViewBag.Pacientes = _pacienteSer.ObtenerTodos();
            ViewBag.Medicos = _medicoSer.ObtenerTodos();
            return View(citas);
        }

        public IActionResult Agregar()
        {
            ViewBag.Pacientes = _pacienteSer.ObtenerTodos();
            ViewBag.Medicos = _medicoSer.ObtenerTodos();
            return View();
        }


        // Formulario — POST
        [HttpPost]
        public IActionResult Agregar(Cita cita)
        {
            bool existe = _citaSer.Agregar(cita);
            if (!existe) { 
            return BadRequest("No se pudo agregar la cita. Verifique los datos ingresados.");
            }
            return RedirectToAction("Index");
        }
        //eliminar
        [Authorize]
        public IActionResult Eliminar(int id)
        {
            _citaSer.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}