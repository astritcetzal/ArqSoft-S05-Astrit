using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace Citas_App.Web.Controllers

{
    public class CitaController : Controller
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;

        public CitaController(ICitaRepository cita, IPacienteRepository paciente, IMedicoRepository medico)
        {
            _citaRepository = cita;
            _pacienteRepository = paciente;
            _medicoRepository = medico;
        }


        public IActionResult Index()
        {
          
            ViewBag.Pacientes = _pacienteRepository.ObtenerTodos();
            ViewBag.Medicos = _medicoRepository.ObtenerTodos();
            return View(_citaRepository.ObtenerTodos());
        }
       
        public IActionResult PorPaciente(int pacienteId)
        {
            var citas = _citaRepository.ObtenerTodos().Where(c => c.PacienteId == pacienteId).ToList();
            ViewBag.Pacientes = _pacienteRepository.ObtenerTodos();
            ViewBag.Medicos = _medicoRepository.ObtenerTodos();
            return View(citas);
        }
        
        public IActionResult Agregar()
        {
            ViewBag.Pacientes = _pacienteRepository.ObtenerTodos();
            ViewBag.Medicos = _medicoRepository.ObtenerTodos();
            return View();
        }

       
        // Formulario — POST
        [HttpPost]
        public IActionResult Agregar(Cita cita)
        {
            _citaRepository.Agregar(cita);
            return RedirectToAction("Index");
        }

    }
}
