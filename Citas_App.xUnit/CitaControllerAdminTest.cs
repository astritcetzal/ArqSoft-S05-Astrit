using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;

public class CitaRepositoryFake : ICitaRepository
{
    private readonly List<Cita> _citas;

    public CitaRepositoryFake(List<Cita> citas) => _citas = citas;

    public List<Cita> ObtenerTodos() => _citas;

    public Cita? ObtenerPorPaciente(int pacienteId)
        => _citas.FirstOrDefault(c => c.PacienteId == pacienteId);

    public void Agregar(Cita cita) => throw new NotImplementedException();

    public Cita? ObtenerPorId(int id) => _citas.FirstOrDefault(c => c.Id == id);

    // MÉTODOS REQUERIDOS POR LA INTERFAZ:
    public void Eliminar(int id) => throw new NotImplementedException();

    public void ConfirmarCita(int id) => throw new NotImplementedException();
}

public class PacienteRepositoryFake : IPacienteRepository
{
    private readonly List<Paciente> _pacientes;

    public PacienteRepositoryFake(List<Paciente> pacientes) => _pacientes = pacientes;

    public List<Paciente> ObtenerTodos() => _pacientes;

    public Paciente? ObtenerPorId(int id) => _pacientes.FirstOrDefault(p => p.Id == id);

    public void Agregar(Paciente paciente) => throw new NotImplementedException();

    // MÉTODO REQUERIDO POR LA INTERFAZ:
    public void Eliminar(int id) => throw new NotImplementedException();
}

public class MedicoRepositoryFake : IMedicoRepository
{
    private readonly List<Medico> _medicos;

    public MedicoRepositoryFake(List<Medico> medicos) => _medicos = medicos;

    public List<Medico> ObtenerTodos() => _medicos;

    public Medico? ObtenerPorId(int id) => _medicos.FirstOrDefault(m => m.Id == id);

    public void Agregar(Medico medico) => throw new NotImplementedException();

    // MÉTODO REQUERIDO POR LA INTERFAZ:
    public void Eliminar(int id) => throw new NotImplementedException();
}