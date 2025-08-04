using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightProject.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public int AirlineId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public string OriginAirportCode { get; set; }
        public string DestinationAirportCode { get; set; }
        public int AvailableSeats { get; set; }

        public decimal Fare { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Airline Airline { get; set; }
        public Airport OriginAirport { get; set; }
        public Airport DestinationAirport { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}