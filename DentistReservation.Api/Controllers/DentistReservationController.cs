using Azure.Core;
using DentistReservation.Api.DatabaseContext;
using DentistReservation.Api.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DentistReservation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DentistReservationController : ControllerBase
    {
        private readonly DentistReservationContext _context;
        private DbSet<Reservation> _reservations;
        
        public DentistReservationController(DentistReservationContext context)
        {
            _context = context;
            _reservations = _context.Set<Reservation>();
        }

        [HttpGet]
        [Route("List")]
        public IActionResult GetAll()
        {
            var list = _reservations.ToList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("Id")]
        public IActionResult GetById(int id)
        {
            return Ok(_reservations.SingleOrDefault(x => x.Id == id));
        }

        [HttpGet]
        [Route("Day")]
        public IActionResult GetByDay(string day)
        {
            try
            {
                DateTime date = DateTime.ParseExact(day, "dd/mm/yyyy", CultureInfo.InvariantCulture);
                var reservations = _reservations.Where(x => x.Date.Day == date.DayOfYear && x.Date.Year == date.Year).ToList();
                return Ok(reservations);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ReservationControl")]
        public IActionResult Control([FromBody] ReservationControlDTO request) 
        {
            try
            {
                DateTime start = DateTime.ParseExact(request.Start, "dd/mm/yyyy", CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact(request.End, "dd/mm/yyyy", CultureInfo.InvariantCulture);

                var reservations = _reservations.Where(res => res.Date >=  start && res.Date <= end).ToList();

                var resDTOs = new List<ReservationControlResponseDTO>();
                foreach (var reservation in reservations)
                {
                    resDTOs.Add(new ReservationControlResponseDTO(reservation));
                }
                return Ok(resDTOs);
            }
            catch (FormatException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Reserve")]
        public IActionResult Post([FromBody] ReservationDTO request)
        {
           
            if (request == null)
            {
                return BadRequest();
            }

            // Input string in "dd/mm/yyyy" format
            string dateString = request.DateFormatted;
            // Specify the exact format of the input string
            string format = "dd/MM/yyyy HH";
            dateString = dateString + " " + request.Hour;

            try
            {
                // Parse the string and convert it to a DateTime object
                DateTime result = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
                
                // validate the result
                if (result.Date < DateTime.Now)
                {
                    return BadRequest("Thats a date back in time");
                }
                if (int.Parse(request.Hour) + request.Duration > 17)
                {
                    return BadRequest("Reservation should be completed earlier than 17");
                }
                if (result.Hour < 9)
                {
                    return BadRequest("Reservation should be made later than 9");
                }
                if (result.DayOfWeek == DayOfWeek.Sunday || result.DayOfWeek == DayOfWeek.Saturday) 
                {
                    return BadRequest("Reservation should be in Weekdays");
                }
                
                var same_day = _reservations.Where(res => res.Date.Day == result.Day).ToList();

                foreach(var res in same_day)
                {
                    if (res.Hour == request.Hour)
                    {
                        return BadRequest("Conflicting date");
                    }
                    if ((int.Parse(res.Hour) + res.Duration > int.Parse(request.Hour)) && (int.Parse(request.Hour) + request.Duration) > (int.Parse(res.Hour) + res.Duration)) {
                        return BadRequest("Conflicting date");
                    }
                    if ((int.Parse(request.Hour) + request.Duration > int.Parse(res.Hour)) && (int.Parse(res.Hour) + res.Duration) > (int.Parse(request.Hour)))
                    {
                        return BadRequest("Conflicting date");
                    }
                }

                _reservations.Add(request.ToReservation(result));
                _context.SaveChanges();

                return Ok(result);
            } catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Put(Reservation reservation) 
        {
            return NotFound();
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var item = _reservations.SingleOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();

            _reservations.Remove(item);
            _context.SaveChanges();
            return Ok();
        }
    }
}
