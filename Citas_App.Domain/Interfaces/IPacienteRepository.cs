using System.Collections.Generic;
using Citas_App.Domain.Models;

namespace Citas_App.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        List<Paciente> ObtenerTodos();
        Paciente? ObtenerPorId(int id);

        void Agregar(Paciente paciente);
        void Eliminar (int id);
        void Editar(Paciente paciente);
    }
}
