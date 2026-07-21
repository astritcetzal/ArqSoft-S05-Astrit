using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citas_App.Application.Interfaces
{
    public interface IPacienteService
    {

        public List<Paciente> ObtenerTodos();
        public Paciente? ObtenerPorId(int id);
        public void Agregar(Paciente paciente);

        public void Eliminar(int id);
    }
}

