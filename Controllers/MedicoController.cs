using Citas_App.Application.Interfaces;
using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace Citas_App.Web.Controllers
{
    public class MedicoController : Controller
    {
        private readonly IMedicoService _medicoSer;
       
        public MedicoController(IMedicoService medico)
        {
            _medicoSer = medico;
            
        }

        public IActionResult Index() => View(_medicoSer.ObtenerTodos());
        
        
        public IActionResult Detalle(int id)
        {
            var medico = _medicoSer.ObtenerPorId(id);
            return medico == null ? NotFound() : View(medico);
        }
        
        public IActionResult Agregar()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Agregar(Medico medico)
        {
            _medicoSer.Agregar(medico);
            return RedirectToAction("Index");
        }
    }

}
