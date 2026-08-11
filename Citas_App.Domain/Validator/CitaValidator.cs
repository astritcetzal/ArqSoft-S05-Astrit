using Citas_App.Domain.Models;
namespace Citas_App.Domain.Validator
{
    public class CitaValidator
    {
        DateTime fechaActual = DateTime.Now;
        public bool Validar(Cita cita)
        {
            if (cita.Fecha.Year < fechaActual.Year) return false;
            if (string.IsNullOrWhiteSpace(cita.Motivo)) return false;
            return true;
        }
    }
}
