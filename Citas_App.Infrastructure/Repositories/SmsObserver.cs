using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;

namespace Citas_App.Infrastructure.Repositories
{
    // cada observer tiene su propia personalidad
    public class SmsObserver: ICitaObserver
    {
        public void OnCitaConfirmada(Cita cita) => Console.WriteLine($" [SMS] Recordatorio enviado al paciente {cita.PacienteId}, cita el {cita.Fecha}  a las {cita.Hora}");
    }
}
