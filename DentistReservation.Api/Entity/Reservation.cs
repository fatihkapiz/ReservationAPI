namespace DentistReservation.Api.Entity
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; } 
        public string Hour { get; set; }
        public DateTime Date { get; set; }

        public Reservation()
        {
            
        }

        public Reservation(int id, string name, int duration, string hour, DateTime date)
        {
            Id = id;
            Name = name;
            Duration = duration;
            Hour = hour;
            Date = date;
        }
    }
}
