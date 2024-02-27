using EsercitazioneStudioMedico.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace EsercitazioneStudioMedico.Context
{
    public class ApplicationDBContext: DbContext
    {
        
        public ApplicationDBContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
