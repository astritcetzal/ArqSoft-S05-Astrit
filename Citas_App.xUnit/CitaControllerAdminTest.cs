using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Citas_App.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Citas_App.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Citas_App.xUnit // Ajustado para quitar la advertencia azul (IDE0130)
{
    // --------------------------------------------------------------------
    // FAKES CORREGIDOS (Regresamos a List<T> como tu los tenías)
    // --------------------------------------------------------------------

    public class CitaRepositoryFake : ICitaRepository
    {
        private readonly List<Cita> _citas;
        public CitaRepositoryFake(List<Cita> citas) => _citas = citas;

        // Usamos List<Cita> exactamente como lo pide tu interfaz
        public List<Cita> ObtenerTodos() => _citas;
        public List<Cita> ObtenerPorPaciente(int pacienteId) => _citas.Where(c => c.PacienteId == pacienteId).ToList();

        public void Agregar(Cita cita) => throw new NotImplementedException();
        public void Eliminar(int id) => throw new NotImplementedException();
        public Cita ObtenerPorId(int id) => throw new NotImplementedException();
        public void ConfirmarCita(int id) => throw new NotImplementedException();

        Cita? ICitaRepository.ObtenerPorPaciente(int pacienteId)
        {
            throw new NotImplementedException();
        }
    }

    public class PacienteRepositoryFake : IPacienteRepository
    {
        private readonly List<Paciente> _pacientes;
        public PacienteRepositoryFake(List<Paciente> pacientes) => _pacientes = pacientes;

        public List<Paciente> ObtenerTodos() => _pacientes;
        public Paciente ObtenerPorId(int id) => _pacientes.FirstOrDefault(p => p.Id == id);

        public void Agregar(Paciente paciente) => throw new NotImplementedException();
        public void Eliminar(int id) => throw new NotImplementedException();
    }

    public class MedicoRepositoryFake : IMedicoRepository
    {
        private readonly List<Medico> _medicos;
        public MedicoRepositoryFake(List<Medico> medicos) => _medicos = medicos;

        public List<Medico> ObtenerTodos() => _medicos;
        public Medico ObtenerPorId(int id) => _medicos.FirstOrDefault(m => m.Id == id);

        public void Agregar(Medico medico) => throw new NotImplementedException();
        public void Eliminar(int id) => throw new NotImplementedException();
    }

    // --------------------------------------------------------------------
    // PRUEBAS
    // --------------------------------------------------------------------

    public class CitaControllerAdminTests
    {
        private CitaController CrearControllerConDatosDePrueba(out List<Cita> citasEsperadas)
        {
            citasEsperadas = new List<Cita>
            {
                new Cita { Id = 1, PacienteId = 10, Estado = "Pendiente" },
                new Cita { Id = 2, PacienteId = 20, Estado = "Confirmada" },
                new Cita { Id = 3, PacienteId = 10, Estado = "Pendiente" }
            };

            var pacientes = new List<Paciente>
            {
                new Paciente { Id = 10, Email = "paciente1@correo.com" },
                new Paciente { Id = 20, Email = "paciente2@correo.com" }
            };

            var medicos = new List<Medico>
            {
                new Medico { Id = 1, Nombre = "Dr. Pérez" }
            };

            // Pasamos los Observers vacíos para cumplir con tu arquitectura
            var observersVacios = new List<ICitaObserver>();
            var citaService = new CitaService(new CitaRepositoryFake(citasEsperadas), observersVacios);

            var pacienteService = new PacienteService(new PacienteRepositoryFake(pacientes));
            var medicoService = new MedicoService(new MedicoRepositoryFake(medicos));

            var controller = new CitaController(citaService, pacienteService, medicoService);

            // Simular usuario admin logueado
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "jorge@admin.com") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            return controller;
        }

        [Fact]
        public void Index_ConCuentaAdmin_RegresaTodasLasCitasSinFiltrar()
        {
            var controller = CrearControllerConDatosDePrueba(out var citasEsperadas);
            var resultado = controller.Index() as ViewResult;

            // Regresamos a List<Cita>
            var modelo = resultado?.Model as List<Cita>;

            Assert.NotNull(modelo);
            Assert.Equal(citasEsperadas.Count, modelo.Count);
        }

        [Fact]
        public void Index_ConCuentaAdmin_IncluyeCitasDeMasDeUnPaciente()
        {
            var controller = CrearControllerConDatosDePrueba(out _);
            var resultado = controller.Index() as ViewResult;

            // Regresamos a List<Cita>
            var modelo = resultado?.Model as List<Cita>;

            Assert.NotNull(modelo);
            var pacientesDistintos = modelo.Select(c => c.PacienteId).Distinct().Count();
            Assert.True(pacientesDistintos > 1);
        }

        [Fact]
        public void Index_ConCuentaAdmin_CargaCatalogosDePacientesYMedicosEnViewBag()
        {
            var controller = CrearControllerConDatosDePrueba(out _);
            controller.Index();

            Assert.NotNull(controller.ViewBag.Pacientes);
            Assert.NotNull(controller.ViewBag.Medicos);
        }
    }
}