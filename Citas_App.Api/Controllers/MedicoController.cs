using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Citas_App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoService _service;

        public MedicoController(MedicoService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult GetAll() => Ok(_service.ObtenerTodos());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var medico = _service.ObtenerPorId(id);
            return medico == null ? NotFound() : Ok(medico);
        }

        [HttpDelete("{Id}")]
        public IActionResult Eliminar(int id)
        {
            var medico = _service.ObtenerPorId(id);

            if (medico == null)
            {
                return NotFound(new { mensaje = $"No se encontró la cita con ID {id}" });
            }

            _service.Eliminar(id);

            return Ok(new { mensaje = "Cita eliminada exitosamente", medico = medico });
        }

    }
}