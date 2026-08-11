using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Cambia el namespace dependiendo de en qué proyecto creaste la carpeta Data
namespace Citas_App.Infrastructure.Data
{
    public class CitasDbContext : IdentityDbContext
    {
        public CitasDbContext(DbContextOptions<CitasDbContext> options)
            : base(options)
        {
        }
    }
}