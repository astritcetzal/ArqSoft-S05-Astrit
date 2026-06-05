using System.Collections.Generic;
using Citas_App.Models;

namespace Citas_App.Interfaces
{
    public interface IPacienteRepository
    {
        List<Paciente> ObtenerTodos();
        Paciente? ObtenerPorId(int id);
        void Agregar(Paciente paciente);
    }
}
