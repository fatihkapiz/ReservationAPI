using DentistReservation.Api.Entity;
using System.Globalization;

namespace DentistReservation.Api
{
    public class ReservationDTO
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Hour { get; set; }
        public string DateFormatted { get; set; }

        public ReservationDTO()
        {

        }

        public ReservationDTO(Reservation res)
        {
            Name = res.Name;
            Duration = res.Duration;
            Hour = res.Hour;
        }

        public Reservation ToReservation(DateTime date)
        {
            return new Reservation()
            {
                Name = this.Name,
                Date = date,
                Duration = this.Duration,
                Hour = this.Hour
            };
        }
    }
}
