using Citas_App.Interfaces;
using Citas_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace Citas_App.Controllers
{
    public class MedicoController : Controller
    {
        private readonly IMedicoRepository _medicoRepository;
       
        public MedicoController(IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository, ICitaRepository citaRepository)
        {
            _medicoRepository = medicoRepository;
            
        }

        public IActionResult Index() => View(_medicoRepository.ObtenerTodos());
        
        
        public IActionResult Detalle(int id)
        {
            var medico = _medicoRepository.ObtenerPorId(id);
            return medico == null ? NotFound() : View(medico);
        }
        
        public IActionResult Agregar()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Agregar(Medico medico)
        {
            _medicoRepository.Agregar(medico);
            return RedirectToAction("Index");
        }
    }

}
