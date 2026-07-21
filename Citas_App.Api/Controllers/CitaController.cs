using Citas_App.Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace Citas_App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitaController : ControllerBase
    {
        private readonly CitaService _citaService;
        
        public CitaController(CitaService citaService)
        {
            _citaService = citaService;
            
        }

        // ¡Ya está encendido de nuevo!
        [HttpGet]
        public IActionResult GetAll() => Ok(_citaService.ObtenerTodos());

        [HttpGet("porpaciente/{pacienteId}")]
        public IActionResult PorPaciente(int pacienteId)
        {
            var cita = _citaService.ObtenerPorPaciente(pacienteId);

            // Si es null regresamos 404, si sí tiene datos regresamos 200 OK
            return cita == null ? NotFound() : Ok(cita);
        }

        // POST: api/cita/confirmar{citaId}   confirmar cita
        [HttpPost("confirmar/{citaId}")]
        public IActionResult ConfirmarCita(int citaId)
        {
            var cita = _citaService.ObtenerPorId(citaId);
            if (cita == null)
            {
                return NotFound(new { mensaje = $"No se encontró la cita con ID {citaId}" });
            }

            _citaService.ConfirmarCita(cita);

            // Si es null regresamos 404, si sí tiene datos regresamos 200 OK
            return Ok(new {mensaje = "Cita comfirmada exitosamente", cita = cita});
        }

        [HttpDelete("{Id}")]
        public IActionResult Eliminar(int id)
        {
            var cita = _citaService.ObtenerPorId(id);

            if (cita == null)
            {
                return NotFound(new { mensaje = $"No se encontró la cita con ID {id}" });
            }

            _citaService.Eliminar(id);

            return Ok(new { mensaje = "Cita eliminada exitosamente", cita = cita });
        }
    }
}