using Microsoft.AspNetCore.Mvc;

namespace Citas_App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculadoraController : ControllerBase
    {
        // GET: api/calculadora/sumar/10/5
        [HttpGet("sumar/{a}/{b}")]
        public IActionResult Sumar(double a, double b)
        {
            var resultado = a + b;
            return Ok(new { operacion = "Suma", a = a, b = b, resultado = resultado });
        }

        // GET: api/calculadora/restar/10/5
        [HttpGet("restar/{a}/{b}")]
        public IActionResult Restar(double a, double b)
        {
            var resultado = a - b;
            return Ok(new { operacion = "Resta", a = a, b = b, resultado = resultado });
        }

        // GET: api/calculadora/multiplicar/10/5
        [HttpGet("multiplicar/{a}/{b}")]
        public IActionResult Multiplicar(double a, double b)
        {
            var resultado = a * b;
            return Ok(new { operacion = "Multiplicación", a = a, b = b, resultado = resultado });
        }
        // GET: api/calculadora/dividir/20/4
        [HttpGet("dividir/{a}/{b}")]
        public IActionResult Dividir(double a, double b)
        {
            // ¡Validación importante para evitar que la API explote!
            if (b == 0)
            {
                return BadRequest(new { error = "No se puede dividir entre cero." });
            }

            var resultado = a / b;
            return Ok(new { operacion = "División", a = a, b = b, resultado = resultado });
        }
    }
}

