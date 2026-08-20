using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
        [Authorize(Roles = "Admin")] // Los médicos no agendan citas
        public IActionResult Agregar()
        {
            // Los médicos siempre se muestran todos para que el paciente elija con quién ir
            ViewBag.Medicos = _medicoSer.ObtenerTodos();

            if (User.IsInRole("Admin"))
            {
                // El administrador ve a todos los pacientes
                ViewBag.Pacientes = _pacienteSer.ObtenerTodos();
            }
            else if (User.IsInRole("Paciente"))
            {
                // El paciente solo se ve a sí mismo
                string correoActual = User.Identity?.Name ?? "";
                var miPerfil = _pacienteSer.ObtenerTodos().FirstOrDefault(p => p.Email == correoActual);

                if (miPerfil != null)
                {
                    // Le mandamos una lista que solo contiene su propio perfil
                    ViewBag.Pacientes = new List<Paciente> { miPerfil };
                }
                else
                {
                    ViewBag.Pacientes = new List<Paciente>();
                }
            }

            return View();
        }

        // Formulario — POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Agregar(Cita cita)
        {
            // SEGURIDAD: Si es paciente, forzamos su ID sin importar lo que mande el formulario
            if (User.IsInRole("Paciente"))
            {
                string correoActual = User.Identity?.Name ?? "";
                var miPerfil = _pacienteSer.ObtenerTodos().FirstOrDefault(p => p.Email == correoActual);

                if (miPerfil != null)
                {
                    cita.PacienteId = miPerfil.Id;
                }
            }

            // Aquí abajo va tu código normal para guardar
            if (ModelState.IsValid)
            {
                _citaSer.Agregar(cita);
                return RedirectToAction("Index");
            }

            // Si hay error, recargamos las listas igual que en el GET
            ViewBag.Medicos = _medicoSer.ObtenerTodos();
            ViewBag.Pacientes = User.IsInRole("Admin") ? _pacienteSer.ObtenerTodos() : new List<Paciente>();

            return View(cita);
        }
        //eliminar
        [Authorize (Roles = "Admin")]
        public IActionResult Eliminar(int id)
        {
            _citaSer.Eliminar(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Editar(int id)
        {
            var cita = _citaSer.ObtenerPorId(id);
            if (cita == null) return NotFound();
            ViewBag.Pacientes = _pacienteSer.ObtenerTodos().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Nombre} {p.Apellido}",
                    Selected = p.Id == cita.PacienteId
                }).ToList();
            ViewBag.Medicos = _medicoSer.ObtenerTodos().Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Nombre} {m.Apellido}",
                    Selected = m.Id == cita.MedicoId
                }).ToList();
            return View("Editar", cita);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Editar(Cita cita)
        {
            var citaExistente = _citaSer.ObtenerPorId(cita.Id);
            if (citaExistente == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.Pacientes = _pacienteSer.ObtenerTodos().Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.Nombre} {p.Apellido}",
                        Selected = p.Id == cita.PacienteId
                    }).ToList();
                ViewBag.Medicos = _medicoSer.ObtenerTodos().Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = $"{m.Nombre} {m.Apellido}",
                        Selected = m.Id == cita.MedicoId
                    }).ToList();
                return View("Editar", cita);
            }
            _citaSer.EditarCita(cita);
            return RedirectToAction( "PorPaciente",new { pacienteId = cita.PacienteId });
        }
    }
}