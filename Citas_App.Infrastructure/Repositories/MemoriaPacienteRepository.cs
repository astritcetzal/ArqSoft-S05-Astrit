using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Citas_App.Infrastructure.Repositories
{
    public class MemoriaPacienteRepository : IPacienteRepository
    {
        private static readonly List<Paciente> _pacientes = new List<Paciente>();
        public List<Paciente> ObtenerTodos()
        {
            return _pacientes;
        }

        public Paciente? ObtenerPorId(int id)
        {

            return _pacientes.FirstOrDefault(c => c.Id == id );
        }

        public void Agregar(Paciente paciente)
        {
            paciente.Id = _pacientes.Count > 0
                ? _pacientes.Max(i => i.Id) + 1
                : 1;
            _pacientes.Add(paciente);
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
