using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Citas_App.Infrastructure.Repositories
{
    public class LoggingPacienteRepository : IPacienteRepository
    {
        //decorator
        private readonly IPacienteRepository _inner;
        public LoggingPacienteRepository(IPacienteRepository pacienteRepository) {
            _inner = pacienteRepository;
        }

        public List<Paciente> ObtenerTodos()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerTodos - inicio");

            var resultado = _inner.ObtenerTodos();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerTodos - inicio ObtenerTodos - {resultado.Count} registros");

            return resultado;
        }
        public Paciente? ObtenerPorId(int id)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerPorId({id}) — inicio");

            var resultado = _inner.ObtenerPorId(id);

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerPorId({id}) — {(resultado != null ? "encontrado" : "no encontrado")}");

            return resultado;
        }

        public void Agregar(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public void Editar(Paciente paciente)
        {
            throw new NotImplementedException();
        }
    }
}
