using Citas_App.Models;
using Microsoft.AspNetCore.Mvc;
using Citas_App.Interfaces;
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
    }

}
