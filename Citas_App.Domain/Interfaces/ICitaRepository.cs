using Citas_App.Domain.Models;

namespace Citas_App.Domain.Interfaces
{
    public interface ICitaRepository
    {
        List<Cita> ObtenerTodos();
        Cita? ObtenerPorPaciente(int pacienteId);
        void Agregar(Cita cita);
        Cita? ObtenerPorId(int id);
        void Eliminar(int id);
        void ConfirmarCita(int id);
        void EditarCita(Cita cita);
    }

}
