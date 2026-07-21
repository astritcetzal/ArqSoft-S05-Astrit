using Citas_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citas_App.Application.Interfaces
{
    public interface IMedicoService
    {
        public List<Medico> ObtenerTodos();

        public Medico? ObtenerPorId(int id);
        public void Agregar(Medico medico);
        public void Eliminar(int id);
    }
}
