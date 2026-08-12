using Citas_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citas_App.Application.Interfaces
{
    public interface ICitaService
    {
        public List<Cita> ObtenerTodos();

        public Cita? ObtenerPorId(int id);
        public Cita? ObtenerPorPaciente(int Id);

        public bool Agregar(Cita cita);

        public void ConfirmarCita(Cita cita);

        public void Eliminar(int id);
        public void EditarCita(Cita cita);
    }
}
