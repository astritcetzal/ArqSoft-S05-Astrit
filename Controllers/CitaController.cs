using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace Citas_App.Web.Controllers

{
    public class CitaController : Controller
    {
        private readonly CitaService _citaSer;
        private readonly PacienteService _pacienteSer;
        private readonly MedicoService _medicoSer;


        public CitaController(CitaService cita, PacienteService paciente, MedicoService medico)
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
            _citaSer.Agregar(cita);
            return RedirectToAction("Index");
        }

    }
}