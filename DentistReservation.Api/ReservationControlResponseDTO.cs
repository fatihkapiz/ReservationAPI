using DentistReservation.Api.Entity;
using System.Globalization;

namespace DentistReservation.Api
{
    public class ReservationControlResponseDTO
    {
        public string Name { get; set; }
        public string Date { get; set; }

        public ReservationControlResponseDTO(Reservation res)
        {
            Name = res.Name;
            Date = res.Date.ToShortDateString();
        }
    }
}
