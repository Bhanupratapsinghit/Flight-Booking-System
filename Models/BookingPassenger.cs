using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.Models
{
    public class BookingPassenger
    {
        [Key]
        public int BookingPassengerId { get; set; }
        public int BookingId { get; set; }
        public int PassengerId { get; set; }
        public string SeatNumber { get; set; }

        // Navigation properties
        public Booking Booking { get; set; }
        public Passenger Passenger { get; set; }
    }
}