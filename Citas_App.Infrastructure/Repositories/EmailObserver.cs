using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;
namespace Citas_App.Infrastructure.Repositories
{
    public class EmailObserver: ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita) => Console.WriteLine($"[Email] Confirmación enviada al paciente {cita.PacienteId} -  con el motivo: {cita.Motivo} - estado: {cita.Estado}");
    }
}
