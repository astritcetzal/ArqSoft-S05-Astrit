using Citas_App.Application.Interfaces;
using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citas_App.Application.Services
{
    public class MedicoService: IMedicoService
    {
        private readonly IMedicoRepository _repo;
        public MedicoService(IMedicoRepository repo)
        {
            _repo = repo;
        }

        public List<Medico> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }
        public Medico? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }
        public void Agregar(Medico medico)
        {
            _repo.Agregar(medico);
        }

        public void Eliminar(int id)
        {
            _repo.Eliminar(id);
        }
    }
}
