using Citas_App.Models;

namespace Citas_App.Interfaces
{
    public interface ICitaRepository
    {
        List<Cita> ObtenerTodos();
        Cita? ObtenerPorPaciente(int pacienteId);
        void Agregar(Cita cita);

    }

}
