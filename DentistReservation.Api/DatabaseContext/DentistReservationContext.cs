using DentistReservation.Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace DentistReservation.Api.DatabaseContext
{
    public class DentistReservationContext : DbContext
    {
        // tablelarımızı buraya koy
        public DbSet<Reservation> reservations { get; set; }

        public DentistReservationContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
