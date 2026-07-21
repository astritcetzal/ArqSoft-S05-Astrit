using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Citas_App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _service;

        public PacienteController(PacienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.ObtenerTodos());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var paciente = _service.ObtenerPorId(id);
            return paciente == null ? NotFound() : Ok(paciente);
        }
        [HttpDelete("{Id}")]
        public IActionResult Eliminar(int id)
        {
            var paciente = _service.ObtenerPorId(id);

            if (paciente == null)
            {
                return NotFound(new { mensaje = $"No se encontró el paciente con ID {id}" });
            }

            _service.Eliminar(id);

            return Ok(new { mensaje = "Paciente eliminado exitosamente", paciente = paciente });
        }
    }


}