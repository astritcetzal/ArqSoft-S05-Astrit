using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace Citas_App.Web.Controllers

{
    [Authorize]
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
            // 1. Obtenemos los catálogos para que la vista pueda mostrar los nombres
            ViewBag.Pacientes = _pacienteSer.ObtenerTodos();
            ViewBag.Medicos = _medicoSer.ObtenerTodos();

            // 2. Traemos TODAS las citas de la base de datos
            var citas = _citaSer.ObtenerTodos();

            // 3. Aplicamos el filtrado dependiendo de quién está conectado
            if (User.IsInRole("Admin"))
            {
                // El Admin ve todo
                return View(citas);
            }
            else if (User.IsInRole("Paciente"))
            {
                string correoActual = User.Identity?.Name ?? "";
                var miPerfil = _pacienteSer.ObtenerTodos().FirstOrDefault(p => p.Email == correoActual);

                if (miPerfil != null)
                {
                    citas = citas.Where(c => c.PacienteId == miPerfil.Id).ToList();
                }
                else citas = new List<Cita>();
            }
            else if (User.IsInRole("Medico"))
            {
                // El Médico solo ve las citas asignadas a él
                string correoActual = User.Identity?.Name ?? "";
                var miPerfil = _medicoSer.ObtenerTodos().FirstOrDefault(m => m.Email == correoActual);

                if (miPerfil != null)
                {
                    citas = citas.Where(c => c.MedicoId == miPerfil.Id).ToList();
                }
                else citas = new List<Cita>();
            }

            // 4. Enviamos la lista final (filtrada o completa) a la misma vista
            return View(citas);
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
        [Authorize (Roles = "Admin")]
        public IActionResult Eliminar(int id)
        {
            _citaSer.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}