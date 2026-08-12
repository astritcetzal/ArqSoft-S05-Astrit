using System.Collections.Generic;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;
using Citas_App.Application.Interfaces;
using Citas_App.Domain.Validator;
namespace Citas_App.Application.Services
{
    // CitaService solo pública - no sabe quien escucha
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _repo;
        //resibe el repo por el constructor
        private readonly IEnumerable<ICitaObserver> _observers; //<-  NET llenará esta lista de manera automatica

        //------ Anterior------
        public CitaService(ICitaRepository repo, IEnumerable<ICitaObserver> observers)
        {
            _repo = repo;
            _observers = observers;
        }
        public List<Cita> ObtenerTodos()=>  _repo.ObtenerTodos();

        public Cita? ObtenerPorId(int id) => _repo.ObtenerPorId(id);
        public Cita? ObtenerPorPaciente(int Id)
        {
            return _repo.ObtenerPorPaciente(Id);
        }
        public bool Agregar(Cita cita)
        {
            var validator = new CitaValidator();
            if (!validator.Validar(cita))
                return false;
            _repo.Agregar(cita);
            return true;
        }

        // ----Nuevo----
        public void ConfirmarCita(Cita cita)
        {
            cita.Estado = "Confirmada";
            // guardar el cambio en la base de datos
            _repo.ConfirmarCita(cita.Id);
            //notificar a todos los interesados
            foreach (var observer in _observers)

                observer.OnCitaConfirmada(cita);
        }

        public void Eliminar(int id)
        {
           _repo.Eliminar(id);
        }
        public void EditarCita(Cita cita)
        {
            _repo.EditarCita(cita);
        }

    }
}
