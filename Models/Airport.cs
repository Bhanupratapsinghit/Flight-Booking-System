using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlightProject.Models
{
    public class Airport
    {
        [Key]
        public string AirportCode { get; set; }
         [Required]
        [StringLength(100)]

        public string AirportName { get; set; }
        [Required]
        [StringLength(100)]
        public string Location { get; set; }
        public string Facilities { get; set; }

        // Navigation properties
        public ICollection<Flight> DepartingFlights { get; set; }
        public ICollection<Flight> ArrivingFlights { get; set; }
    }
}