using DentistReservation.Api.Entity;
using System.Globalization;

namespace DentistReservation.Api
{
    public class ReservationControlDTO
    {
        public string Start { get; set; }
        public string End { get; set; }

        public ReservationControlDTO(string start, string end)
        {
            Start = start;
            End = end;
        }
    }
}
