using Citas_App.Models;
namespace Citas_App.Interfaces
{
    public interface IMedicoRepository
    {
        List<Medico> ObtenerTodos();
        Medico? ObtenerPorId(int id);
        //Void agregar(Medico medico)
    }
}
